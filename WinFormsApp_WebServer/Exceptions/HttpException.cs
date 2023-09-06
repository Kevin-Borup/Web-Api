using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp_WebServer.Exceptions
{
    internal class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public HttpException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
}
