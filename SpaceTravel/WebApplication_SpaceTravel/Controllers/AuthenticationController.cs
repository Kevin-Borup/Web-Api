using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using WebApplication_SpaceTravel.DataHandlers;
using WebApplication_SpaceTravel.DTOs;
using WebApplication_SpaceTravel.Exceptions;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Controllers
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
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("GenerateKey")]
        public string GenerateApiKey([FromQuery] string title)
        {
            string identifier = _encryptor.GenerateIdentifier();

            while (_dataHandler.GetKeyIfIdentifierExists(identifier).Result is not null)
            {
                identifier = _encryptor.GenerateIdentifier();
            }
            
            var apiIdentifierSalt = _config[APIKEYIDENTIFIERSALT];

            RouteKey route = _encryptor.GenerateKey(identifier, apiIdentifierSalt, out string apikey);
            route.Title = title;

            _dataHandler.InsertRouteKey(route);

            return new RouteKeyDTO(identifier, apikey).ToString();
        }
    }
}
