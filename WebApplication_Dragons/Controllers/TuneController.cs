using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication_Dragons.DataHandlers;
using WebApplication_Dragons.Models;

namespace WebApplication_Dragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task PostTune([FromQuery] Tune tune)
        {
            if (tune == null) throw new HttpRequestException("No tune parameters", null, HttpStatusCode.BadRequest);
            if (tune.Index == null) throw new HttpRequestException("No index parameters", null, HttpStatusCode.BadRequest);
            if (tune.Name == null) throw new HttpRequestException("No name parameters", null, HttpStatusCode.BadRequest);
            if (tune.Duration == null) throw new HttpRequestException("No duration parameters", null, HttpStatusCode.BadRequest);

            await _dataHandler.InsertNewTune(tune);
        }
    }
}
