﻿using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using WebApplication_SpaceTravel.Exceptions;

namespace WebApplication_SpaceTravel.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (TitleException tEx)
            {
                await HandleTitleExceptionAsync(httpContext, tEx);
            }
            catch (HttpException hEx)
            {
                await HandleHttpExceptionAsync(httpContext, hEx);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(httpContext);
            }
        }
        private async Task HandleTitleExceptionAsync(HttpContext context, TitleException tEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var message = "Bad Title parameter - " + tEx.GivenTitle + " is not a valid title";

            await context.Response.WriteAsync(context.Response.StatusCode.ToString() + " - " + message);
        }


        private async Task HandleHttpExceptionAsync(HttpContext context, HttpException hEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)hEx.StatusCode;
            string statusCode = hEx.StatusCode.ToString();
            var message = "";

            switch (hEx.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    break;
                case HttpStatusCode.Unauthorized:
                    break;
                case HttpStatusCode.Forbidden:
                    break;
                case HttpStatusCode.TooManyRequests:
                    break;
                default:
                    break;
            }

            await context.Response.WriteAsync(statusCode + " - " + message);

        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(context.Response.StatusCode.ToString() + " - Internal Server Error.");
        }
    }
}