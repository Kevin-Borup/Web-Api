using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_SpaceTravel.DataHandlers;

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



    }
}
