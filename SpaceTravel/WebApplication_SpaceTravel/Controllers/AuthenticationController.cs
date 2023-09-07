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
        IEncryptionService _encryptor;
        IDataHandler _dataHandler;

        public AuthenticationController(IDataHandler dataHandler, IEncryptionService encryption)
        {
            _encryptor = encryption;
            _dataHandler = dataHandler;
        }

        [HttpGet("GenerateKey")]
        public string GenerateApiKey([FromQuery] string title)
        {
            string identifier = _encryptor.GenerateIdentifier(title);

            while (_dataHandler.GetKeyIfIdentifierExists(identifier) is not null)
            {
                identifier = _encryptor.GenerateIdentifier(title);
            }

            RouteKey route = _encryptor.GenerateKey(identifier);
            route.Title = title;

            _dataHandler.InsertRouteKey(route);

            return new RouteKeyDTO(route).ToString();
        }
    }
}
