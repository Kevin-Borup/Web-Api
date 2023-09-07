using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Dragons.Models;

namespace WebApplication_Dragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("NewDragon")]
        public async Task CreateNewDragon(Account newUser)
        {

        }

        [HttpGet("Login")]
        public IActionResult LoginDragon(Account user)
        {

        }
    }
}
