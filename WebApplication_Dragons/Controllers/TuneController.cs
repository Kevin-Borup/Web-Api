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

        [Authorize(Roles = "Listener, Creator")]
        [HttpGet("DragonTunes")]
        public async Task<IEnumerable<Tune>> GetAllTunes()
        {
            return await _dataHandler.GetAllTunes();
        }

        [Authorize(Roles = "Creator")]
        [HttpPost("NewDragonTune")]
        public async Task PostTune(Tune tune)
        {
            await _dataHandler.InsertNewTune(tune);
        }
    }
}
