using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_SpaceTravel.DataHandlers;
using WebApplication_SpaceTravel.Exceptions;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceTravelController : ControllerBase
    {
        private readonly MongoDataHandler _dataHandler;

        public SpaceTravelController(MongoDataHandler dataHandler)
        {
            this._dataHandler = dataHandler;
        }

        //Validated by middleware
        [HttpGet("GetRoutes")]
        public async Task<IEnumerable<GalacticRoute>> GetGalacticRoutes()
        {
            var returnValue = new List<GalacticRoute>();

            if (HttpContext.Session.IsAvailable)
            {
                string? title = HttpContext.Session.GetString("SessionTitle");

                if (title == null) return returnValue;

                if (title == "Captain")
                {
                    return await _dataHandler.GetAll();
                } else if (title == "Cadet")
                {
                    return await _dataHandler.GetWithinYear();
                }
            }

            return returnValue;
        }

    }
}
