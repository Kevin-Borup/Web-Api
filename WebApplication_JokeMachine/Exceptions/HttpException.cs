using System.Net;

namespace WebApplication_JokeMachine.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public HttpException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
}
