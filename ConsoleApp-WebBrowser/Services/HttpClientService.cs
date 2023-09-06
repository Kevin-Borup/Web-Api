using ConsoleApp_WebBrowser.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_WebBrowser.Services
{
    internal class HttpClientService : IClientService
    {
        HttpClient client = new HttpClient();

        public HttpClientService()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetFromWeb()
        {
            HttpResponseMessage response = await client.GetAsync(new Uri("http://www.dr.dk/"));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "";
            }
        }
    }
}
