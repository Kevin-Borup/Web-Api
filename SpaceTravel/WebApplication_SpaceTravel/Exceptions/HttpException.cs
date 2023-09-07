using System.Net;

namespace WebApplication_SpaceTravel.Exceptions
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
