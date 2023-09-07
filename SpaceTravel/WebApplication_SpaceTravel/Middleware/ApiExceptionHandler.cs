using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using WebApplication_SpaceTravel.Exceptions;

namespace WebApplication_SpaceTravel.Middleware
{
    public class ApiExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Catches any errors, a global error handler
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            if (context.Exception is TitleException)
            {
                var tEx = context.Exception as TitleException;
                HandleTitleExceptionAsync(context, tEx);
            }
            else if (context.Exception is HttpException)
            {
                var hEx = context.Exception as HttpException;
                HandleHttpExceptionAsync(context, hEx);
            }
            else
            {
                HandleExceptionAsync(context);
            }
        }

        /// <summary>
        /// Manages the errors from a wrongful title value
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tEx"></param>
        private void HandleTitleExceptionAsync(ExceptionHandlerContext context, TitleException tEx)
        {
            var message = ((int)HttpStatusCode.BadRequest).ToString() + " | Bad Title parameter - " + tEx.GivenTitle + " is not a valid title";
            context.Request.CreateResponse(HttpStatusCode.BadRequest,  message);
        }

        /// <summary>
        /// Manages all thrown HttpExceptions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hEx"></param>
        private void HandleHttpExceptionAsync(ExceptionHandlerContext context, HttpException hEx)
        {
            string statusCode = hEx.StatusCode.ToString();
            var message = "";

            switch (hEx.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    message = "Bad Request, missing api key to grant access";
                    break;
                case HttpStatusCode.Unauthorized:
                    message = "Unauthorized, key doesn't grant access";
                    break;
                case HttpStatusCode.TooManyRequests:
                    message = "Cadets are limited to 5 request per half hour. You have exceeded this. Request denied.";
                    break;
                case HttpStatusCode.InternalServerError:
                    message = "An internal server error occured.";
                    break;
                default:
                    break;
            }

            message = statusCode.ToString() + " | " + message;
            context.Request.CreateResponse(hEx.StatusCode, message);
        }

        /// <summary>
        /// Manages any other exception, showcases the error as an internal error. Uncaught errors.
        /// </summary>
        /// <param name="context"></param>
        private void HandleExceptionAsync(ExceptionHandlerContext context)
        {
            var message = ((int)HttpStatusCode.InternalServerError).ToString() + " | Internal Server Error.";
            context.Request.CreateResponse(HttpStatusCode.InternalServerError, message);
        }


    }
}
