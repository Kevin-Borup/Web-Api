using ConsoleApp_WebBrowser.Interfaces;
using ConsoleApp_WebBrowser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_WebBrowser.Controller
{
    internal class WebBrowserController
    {
        public async Task Run()
        {
            IPrinter cPrinter = new ConsolePrinter();
            IClientService clientService = new HttpClientService();


            string response = await clientService.GetFromWeb();
            cPrinter.Print(response);
        }
    }
}
