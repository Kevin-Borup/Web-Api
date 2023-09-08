using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication_MilkAndCookies.Controllers.Opgave1_e_f
{
    [Route("api/[controller]")]
    [ApiController]
    public class HisNameController : ControllerBase
    {
        [HttpGet]
        public string GetHisName()
        {
            string? name = Request.Cookies["name"];

            return name ?? "";
        }
    }
}
