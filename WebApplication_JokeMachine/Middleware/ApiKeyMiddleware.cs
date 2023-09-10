using System.Net.Mail;
using System.Net;
using WebApplication_JokeMachine.Exceptions;
using WebApplication_JokeMachine.Models;
using WebApplication_JokeMachine.Interfaces;

namespace WebApplication_JokeMachine.Middleware
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
            if (context.Request.Path.Equals("/GenerateKey"))
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
            var InputIdentifier = key[0];
            var InputKey = key[1];

            // Hash the identifier with the local salt. To create a comparable value with the one stored on the DB
            string hashedIden = _encryption.HashIdentifier(InputIdentifier, apiIdentifierSalt);

            ApiKey? apiKey = _dataHandler.GetKeyIfIdentifierExists(hashedIden); // Compare the hashed identifier to all keys.

            // if a key is found, and the inputtet key also match the one on the object, then the api key is valid.
            if (apiKey != null && _encryption.CheckKey(InputKey, apiKey.Key, apiKey.KeySalt))
            {
                if (context.Session.IsAvailable)
                {
                    context.Session.SetString("SessionAccess", apiKey.AccessLevel); //Save the access here
                    await _next(context);
                }
            }
            else throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}
