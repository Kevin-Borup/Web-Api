using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Dragons.DataHandlers;
using WebApplication_Dragons.Models;

namespace WebApplication_Dragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TuneController : ControllerBase
    {
        MongoDataHandler _dataHandler;
        public TuneController()
        {
            _dataHandler = new MongoDataHandler();
        }

        [Authorize]
        [HttpGet("DragonTunes")]
        public async Task<IEnumerable<Tune>> GetAllTunes()
        {
            _dataHandler.
        }

        [Authorize]
        [HttpPost("NewDragonTune")]
        public async Task PostTune()
        {

        }
    }
}
