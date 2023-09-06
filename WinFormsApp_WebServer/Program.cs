using WinFormsApp_WebServer.Controllers;
using WinFormsApp_WebServer.Interfaces;
using WinFormsApp_WebServer.Services;

namespace WinFormsApp_WebServer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        
        static void Main()
        {
            IWebServer server = new WebServerService();
            IWebController controller = new WebServerController(server);
            controller.Run();
        }
    }
}