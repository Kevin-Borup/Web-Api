using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_SpaceTravel.DataHandlers;
using WebApplication_SpaceTravel.Exceptions;
using System.Net;
using WebApplication_SpaceTravel.Models;
using WebApplication_SpaceTravel.Interfaces;

namespace WebApplication_SpaceTravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceTravelController : ControllerBase
    {
        private readonly IDataHandler _dataHandler;

        public SpaceTravelController(IDataHandler dataHandler)
        {
            this._dataHandler = dataHandler;
        }

        /// <summary>
        /// Returns the Galactic Routes, 
        /// Cadets are limited to routes that finish within the year.
        /// Captains can see all routes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpException"></exception>
        [HttpGet("GetRoutes")]
        public async Task<IEnumerable<GalacticRoute>> GetGalacticRoutes()
        {
            if (HttpContext.Session.IsAvailable)
            {
                string? title = HttpContext.Session.GetString("SessionTitle");

                if (title == null) throw new HttpException(HttpStatusCode.InternalServerError);

                if (title == "Captain")
                {
                    return await _dataHandler.GetAll();
                } else if (title == "Cadet")
                {
                    return await _dataHandler.GetWithinYear();
                }
            }

            throw new HttpException(HttpStatusCode.InternalServerError);
        }

    }
}
