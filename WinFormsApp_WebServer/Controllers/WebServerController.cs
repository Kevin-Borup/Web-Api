using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp_WebServer.Interfaces;
using WinFormsApp_WebServer.Services;
using WinFormsApp_WebServer.Views;

namespace WinFormsApp_WebServer.Controllers
{
    internal class WebServerController : IWebController
    {
        IWebServer server = new WebServerService();

        public WebServerController(IWebServer webServer)
        {
            this.server = webServer;
        }

        /// <summary>
        /// Runs the form, managing the server
        /// </summary>
        [STAThread]
        public void Run()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new WebServerForm(this));
        }

        // These as event listeners would be better
        public bool StartServer(int port)
        {
            return server.Start(port);
        }

        public void StopServer()
        {
            server.Stop();
        }
    }
}
