using ConsoleApp_WebBrowser.Services;
using ConsoleApp_WebBrowser.Controller;

namespace ConsoleApp_WebBrowser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WebBrowserController controller = new WebBrowserController();
            await controller.Run();
        }
    }
}