using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp_WebServer.Views;

namespace WinFormsApp_WebServer.Interfaces
{
    internal interface IWebController
    {
        void Run();
        bool StartServer(int port);
        void StopServer();
    }
}
