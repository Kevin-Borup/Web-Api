using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication_MilkAndCookies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyNameController : ControllerBase
    {
        [HttpGet]
        public string GetMyName(string name)
        {
            CookieOptions co = new CookieOptions();
            co.MaxAge = TimeSpan.FromMinutes(5);

            if (name.Equals("clear"))
            {
                co.MaxAge = TimeSpan.Zero;
            }

            Response.Cookies.Append(nameof(name), name, co);

            return name;
        }
    }
}
