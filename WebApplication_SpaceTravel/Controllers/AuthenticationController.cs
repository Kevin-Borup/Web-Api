using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using WebApplication_SpaceTravel.DTOs;
using WebApplication_SpaceTravel.Exceptions;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        Encryptor encryptor;

        public AuthenticationController()
        {
            encryptor = new Encryptor();
        }

        [HttpGet("GenerateKey")]
        public string GenerateApiKey([FromQuery] string title)
        {
            RouteKey route = encryptor.GenerateKey(title);

            return new RouteKeyDTO(route).ToString();
        }
    }
}
