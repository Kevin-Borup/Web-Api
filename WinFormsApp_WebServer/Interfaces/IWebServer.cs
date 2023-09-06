using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp_WebServer.Interfaces
{
    internal interface IWebServer
    {
        bool Start(int port);
        void Stop();
    }
}
