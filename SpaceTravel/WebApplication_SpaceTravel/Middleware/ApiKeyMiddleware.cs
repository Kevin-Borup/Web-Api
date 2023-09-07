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
        private readonly IEncryptionService _encryption;
        private readonly IDataHandler _dataHandler;

        public ApiKeyMiddleware(RequestDelegate next, IEncryptionService encryption, IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
            _encryption = encryption;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.IsAvailable) { context.Session.SetString("SessionTitle", ""); }

            // If the request is targeting the generatekey api, let them pass.
            if (context.Request.Path.Equals("/api/Authentication/GenerateKey"))
            {
                await _next(context);
                return;
            }

            //Whether an apikey header can be found
            if (!context.Request.Headers.TryGetValue(APIKEY, out var ApiFullKey)) throw new HttpException(HttpStatusCode.BadRequest);

            //Gets the locally saved identifier salt from appsettings.json
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiIdentifierSalt = appSettings.GetValue<string>(APIKEYIDENTIFIERSALT);

            var key = ApiFullKey[0].Split("."); //Split identifier from key at .
            var ApiIdentifier = key[0];
            var ApiKey = key[1];

            // Hash the identifier with the local salt. To create a comparable value with the one stored on the DB
            string hashedIden = _encryption.HashIdentifier(ApiIdentifier, apiIdentifierSalt);

            RouteKey? routeKey = await _dataHandler.GetKeyIfIdentifierExists(hashedIden); // Compare the hashed identifier to all keys.

            // if a key is found, and the inputtet key also match the one on the object, then the api key is valid.
            if (routeKey != null && _encryption.CheckKey(ApiKey, routeKey.Key, routeKey.KeySalt))
            {
                //If the key is of cadet, then check if the firstquery time was over 30 min ago, if so, set the time to now, and reset the counter.
                if (routeKey.Title.Equals("Cadet") && (DateTime.Now.ToUniversalTime() - routeKey.FirstQuery).TotalMinutes > 30)
                {
                    routeKey.FirstQuery = DateTime.Now.ToUniversalTime();
                    routeKey.QueryCount = 0;
                }

                routeKey.QueryCount++; // used to log behavior of cadets and captains. Only cadets are limited by this.
                 
                await _dataHandler.UpdateKeyQueryData(routeKey);

                // if the key is of a cadet type, and the querycounter is on 5 or above, block access.
                if (routeKey.Title.Equals("Cadet") && routeKey.QueryCount >= 5) throw new HttpException(HttpStatusCode.TooManyRequests);


                if (context.Session.IsAvailable)
                {
                    context.Session.SetString("SessionTitle", routeKey.Title); //Save the title here
                    await _next(context);
                }
            }
            else throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}
