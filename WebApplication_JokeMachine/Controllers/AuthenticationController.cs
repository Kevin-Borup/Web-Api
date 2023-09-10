using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_JokeMachine.DTOs;
using WebApplication_JokeMachine.Interfaces;
using WebApplication_JokeMachine.Models;
using System.Net;

namespace WebApplication_JokeMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string APIKEYIDENTIFIERSALT = "XApiIdentifierSalt";

        IConfiguration _config;
        IEncryptionService _encryptor;
        IDataHandler _dataHandler;

        public AuthenticationController(IConfiguration configuration, IDataHandler dataHandler, IEncryptionService encryption)
        {
            _config = configuration;
            _encryptor = encryption;
            _dataHandler = dataHandler;
        }

        /// <summary>
        /// Generate an api key, guarantees a unique identifier. Returns the identifier.key.
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet("GenerateKey")]
        public string GenerateApiKey([FromQuery] string accessLevel)
        {
            if (string.IsNullOrWhiteSpace(accessLevel)) throw new HttpRequestException("No access level specified", null, HttpStatusCode.BadRequest);

            string identifier = _encryptor.GenerateIdentifier();

            while (_dataHandler.GetKeyIfIdentifierExists(identifier) is not null)
            {
                identifier = _encryptor.GenerateIdentifier();
            }

            var apiIdentifierSalt = _config[APIKEYIDENTIFIERSALT];

            ApiKey apiKey = _encryptor.GenerateKey(identifier, apiIdentifierSalt, out string presentedKey);
            apiKey.AccessLevel = accessLevel;

            _dataHandler.InsertApiKey(apiKey);

            return new ApiKeyDTO(identifier, presentedKey).ToString();
        }
    }
}
