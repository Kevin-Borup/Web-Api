using WebApplication_SpaceTravel.Exceptions;
using System.Net;
using WebApplication_SpaceTravel.Models;
using WebApplication_SpaceTravel.DataHandlers;
using Microsoft.AspNetCore.Http.Extensions;
using ZstdSharp;
using WebApplication_SpaceTravel.Interfaces;

namespace WebApplication_SpaceTravel.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "XApiKey";
        private const string APIKEYIDENTIFIERSALT = "XApiIdentifierSalt";
        private readonly IEncryptionService _encryptor;
        private readonly IDataHandler _dataHandler;

        public ApiKeyMiddleware(RequestDelegate next, IEncryptionService encryption, IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
            _encryptor = encryption;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.IsAvailable) { context.Session.SetString("SessionTitle", ""); }

            if (context.Request.GetEncodedUrl().ToLower().Contains("api/Authentication/GenerateKey"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEY, out var ApiFullKey)) throw new HttpException(HttpStatusCode.BadRequest);

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiIdentifierSalt = appSettings.GetValue<string>(APIKEYIDENTIFIERSALT);

            var key = ApiFullKey[0].Split(".");
            var ApiIdentifier = key[0];
            var ApiKey = key[1];

            string hashedIden = _encryptor.HashIdentifier(ApiIdentifier, apiIdentifierSalt);

            RouteKey? routeKey = await _dataHandler.GetKeyIfIdentifierExists(hashedIden);

            if (routeKey != null && _encryptor.CheckKey(ApiKey, routeKey.Key, routeKey.KeySalt))
            {
                if (routeKey.Title.Equals("Cadet") && (routeKey.FirstQuery - DateTime.Now).TotalMinutes > 30)
                {
                    routeKey.FirstQuery = DateTime.Now;
                    routeKey.QueryCount = 0;
                }

                routeKey.QueryCount++;

                await _dataHandler.UpdateKeyQueryData(routeKey);

                if (routeKey.Title.Equals("Cadet") && routeKey.QueryCount >= 5) throw new HttpException(HttpStatusCode.TooManyRequests);


                if (context.Session.IsAvailable)
                {
                    context.Session.SetString("SessionTitle", routeKey.Title);
                    await _next(context);
                }
            }
            else throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}
