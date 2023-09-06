using Microsoft.AspNetCore.Mvc;

namespace WebApplication_MilkAndCookies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MilkshakeController : ControllerBase
    {
        [HttpGet()]
        public string Get(string favoriteMilkshake)
        {
            CookieOptions co = new CookieOptions();
            co.MaxAge = TimeSpan.FromMinutes(5);

            Response.Cookies.Append(nameof(favoriteMilkshake), favoriteMilkshake, co);

            return favoriteMilkshake;
        }

        [HttpGet("[action]")]
        public string GetCookie()
        {
            string? favoriteMilkshake = Request.Cookies["favoriteMilkshake"];

            return favoriteMilkshake ?? "";
        }
    }
}