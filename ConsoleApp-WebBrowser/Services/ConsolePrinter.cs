using ConsoleApp_WebBrowser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp_WebBrowser.Services
{
    internal class ConsolePrinter : IPrinter
    {
        public void Print(string webResponse) 
        {
            string response = CleanTags(webResponse);

            Console.WriteLine(response);
        }

        private string CleanTags(string webResponse)
        {
            return Regex.Replace(webResponse, "<[^>]*>", String.Empty);
        }
    }
}
