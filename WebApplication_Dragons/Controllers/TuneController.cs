using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication_Dragons.DataHandlers;
using WebApplication_Dragons.DTOs;
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
        public async Task<IEnumerable<TuneDTO>> GetAllTunes()
        {
            var tunes = await _dataHandler.GetAllTunes();
            List<TuneDTO> tunesDTO = new List<TuneDTO>();

            tunes.ToList().ForEach(tune => tunesDTO.Add(new TuneDTO(tune)));

            return tunesDTO;
        }

        [Authorize(Roles = "Creator")]
        [HttpPost("NewDragonTune")]
        public async Task PostTune([FromQuery] TuneDTO tune)
        {
            if (tune == null) throw new HttpRequestException("No tune parameters", null, HttpStatusCode.BadRequest);
            if (tune.Name == null) throw new HttpRequestException("No name parameters", null, HttpStatusCode.BadRequest);
            if (tune.Duration == null) throw new HttpRequestException("No duration parameters", null, HttpStatusCode.BadRequest);

            int highestIndex = await _dataHandler.HighestTuneIndex();
            highestIndex++; 

            await _dataHandler.InsertNewTune(new Tune(tune, highestIndex));
        }
    }
}
