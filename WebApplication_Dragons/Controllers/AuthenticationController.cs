using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using WebApplication_Dragons.DataHandlers;
using WebApplication_Dragons.DTOs;
using WebApplication_Dragons.Models;
using WebApplication_Dragons.Services;

namespace WebApplication_Dragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        MongoDataHandler _dataHandler;
        Encryptor _encryptor;
        IConfiguration _config;

        public AuthenticationController(IConfiguration configuration)
        {
            _dataHandler = new MongoDataHandler();
            _encryptor = new Encryptor();
            this._config = configuration;
        }

        [AllowAnonymous] // Not neccesarry, but an explicit definition
        [HttpPost("NewDragon")]
        public async Task CreateNewDragon(AccountDTO newUser, string role)
        {
            if (newUser == null) throw new HttpRequestException("No user parameters", null, HttpStatusCode.BadRequest);
            if (newUser.Username == null) throw new HttpRequestException("No user parameters", null, HttpStatusCode.BadRequest);
            if (newUser.Password == null) throw new HttpRequestException("No pass parameters", null, HttpStatusCode.BadRequest);
            if (role == null) throw new HttpRequestException("No role parameters", null, HttpStatusCode.BadRequest);

            if (await _dataHandler.IsUsernameUsed(newUser.Username)) throw new HttpRequestException("Name is already in use", null, HttpStatusCode.Conflict);

            _encryptor.EncryptPassword(newUser.Password, out byte[] hashedPass, out byte[] passSalt);

            await _dataHandler.CreateNewUser(new Account()
            {
                Username = newUser.Username,
                Password = Convert.ToBase64String(hashedPass),
                PassSalt = Convert.ToBase64String(passSalt),
                AccountRoles = new string[] { role }
            });
        }

        [AllowAnonymous] // Not neccesarry, but an explicit definition
        [HttpGet("Login")]
        public async Task<IActionResult> LoginDragon(AccountDTO login)
        {
            if (login == null) return Unauthorized();
            if (login.Username == null) return Unauthorized();
            if (login.Password == null) return Unauthorized();

            Account? user = await _dataHandler.GetUser(login.Username);

            if (user == null) return Unauthorized();

            
            if (!_encryptor.CheckPassword(login.Password, Convert.FromBase64String(user.Password), Convert.FromBase64String(user.PassSalt))) return Unauthorized();

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claims = new List<Claim>();

            for (int i = 0; i < user.AccountRoles.Length; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.AccountRoles[i]));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new Token { JWT = jwtToken });
        }
    }
}
