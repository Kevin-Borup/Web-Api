using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using WinFormsApp_WebServer.Exceptions;
using WinFormsApp_WebServer.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace WinFormsApp_WebServer.Services
{
    internal class WebServerService : IWebServer
    {
        private bool serverRunning;
        private int ttl = 8;
        private Encoding uft8 = Encoding.UTF8;
        private Socket socket;
        private string contentPath = "./WebContent/";
        private string errorPath => contentPath + "Errors/";
        private Dictionary<string, string> extensions = new Dictionary<string, string>()
        { 
            //{ "extension", "content type" }
            { "htm", "text/html" },
            { "html", "text/html" }
        };

        /// <summary>
        /// Setting up the server socket, using the localhost address, and specified port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool SetupServer(int port)
        {
            if (serverRunning) return false;

            try
            {
                // An IPv4 socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Loopback, port));
                socket.Listen();
                socket.ReceiveTimeout = ttl;
                socket.SendTimeout = ttl;
                serverRunning = true;
            }
            catch { return false; } // Failed to create socket

            return true;
        }

        /// <summary>
        /// Starts the server, making it ready to recieve requests
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Start(int port)
        {
            if(!SetupServer(port)) return false;

            //Create a main continuous thread
            Thread requestListenerT = new Thread(() =>
            {
                while (serverRunning)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = socket.Accept();
                        // this main continuous thread, now creates a thread per request.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = ttl;
                            clientSocket.SendTimeout = ttl;
                            try 
                            { 
                                HandleTheRequest(clientSocket); 
                            }
                            catch(HttpException httpEx) 
                            {
                                switch (httpEx.StatusCode)
                                {
                                    //Could implement all sorts of error handling here
                                    case HttpStatusCode.BadRequest:
                                        break;
                                    case HttpStatusCode.Unauthorized:
                                        break;
                                    case HttpStatusCode.NotFound:
                                        // We don't have the requested page, either because of extension type or page name.
                                        SendNotFoundResponse(clientSocket); 
                                        break;
                                    case HttpStatusCode.NotImplemented:
                                        // If the requested method isn't implemented
                                        SendNotImplementedResponse(clientSocket); 
                                        break;
                                    default:
                                        break;
                                }
                                try { clientSocket.Close(); } catch { }
                            }
                            catch
                            {
                                try { clientSocket.Close(); } catch { }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Start(); //Starts the server listening thread.

            return true; // The server is now up and running, waiting for connections.
        }

        /// <summary>
        /// Handles all recieved http requests. 
        /// Reads it from the socket buffer, dissects the requests, and replies with the appropriate reponse.
        /// </summary>
        /// <param name="clientSocket"></param>
        private void HandleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb buffer
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = uft8.GetString(buffer, 0, receivedBCount); //convert from byte to string

            // Get http method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;

            //get requested ressource from the url
            string requestedUrl = strReceived.Substring(start, length);

            string requestedFile;
            if (httpMethod.Equals("GET"))
                requestedFile = requestedUrl.Split('?')[0];
            else
            {
                throw new HttpException(HttpStatusCode.NotImplemented);
            }

            requestedFile = requestedFile.Replace("/", @"\").Replace("\\..", "");
            start = requestedFile.LastIndexOf('.') + 1;
            if (start > 0)
            {
                length = requestedFile.Length - start;
                string extension = requestedFile.Substring(start, length);
                if (extensions.ContainsKey(extension)) // If the MIME type is supported by the server
                    if (File.Exists(contentPath + requestedFile)) //If yes check existence of the file
                        SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile), extensions[extension]);
                    else throw new HttpException(HttpStatusCode.NotFound);
            }
            else
            {
                // If file is not specified try to send Home.html
                if (requestedFile.Substring(length - 1, 1) != @"\")
                    requestedFile += @"\";

                else if (File.Exists(contentPath + requestedFile + "\\Home.html"))
                    SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile + "\\Home.html"), "text/html");
                else throw new HttpException(HttpStatusCode.NotFound);
            }
        }

        public void Stop()
        {
            if (serverRunning)
            {
                // Disposal of ressources
                try { socket.Close(); }
                catch { }
                socket = null;
                serverRunning = false;
            }
        }

        //Status Responses
        private void SendNotImplementedResponse(Socket clientSocket)
        {
            SendResponse(clientSocket, File.ReadAllBytes(errorPath + 501 + ".html"), "501 Not Implemented", "text/html");
        }

        private void SendNotFoundResponse(Socket clientSocket)
        {
            SendResponse(clientSocket, File.ReadAllBytes(errorPath + 404 +".html"), "404 Not Found", "text/html");
        }

        private void SendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            SendResponse(clientSocket, bContent, "200 OK", contentType);
        }

        /// <summary>
        /// Sends a http utf8 encoded response, the client socket is immediately closed after the response is sent.
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="bContent"></param>
        /// <param name="responseCode"></param>
        /// <param name="contentType"></param>
        private void SendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                                  string contentType)
        {
            try
            {
                byte[] bHeader = uft8.GetBytes(
                                    "HTTP/2 " + responseCode + "\r\n"
                                  + "Server: My Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch { }
        }
    }
}
