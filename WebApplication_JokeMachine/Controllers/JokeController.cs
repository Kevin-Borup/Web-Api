using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication_JokeMachine.DataHandlers;
using WebApplication_JokeMachine.Extenders;
using WebApplication_JokeMachine.Models;


namespace WebApplication_JokeMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        LocalDataHandler _dataHandler;

        public JokeController()
        {
            _dataHandler = new LocalDataHandler();
        }

        [HttpGet]
        public async Task<Joke> GetJoke([FromQuery] string category = "")
        {
            string lang = Request.Headers.ContentLanguage;

            if (HttpContext.Session.IsAvailable)
            {
                List<Joke> allJokes;
                if (string.IsNullOrWhiteSpace(category))
                {
                   allJokes = _dataHandler.GetAllJokes(lang);
                }
                else
                {
                    allJokes = _dataHandler.GetJokesInCategory(lang, category);
                }

                if (allJokes == null || allJokes.Count == 0) throw new HttpRequestException("No jokes left", null, HttpStatusCode.NotFound);

                var usedJokesIndex = HttpContext.Session.GetObjectFromJson<List<int>>("UsedJokesIndex");
                if (usedJokesIndex == null) usedJokesIndex= new List<int>();

                var unusedJokes = allJokes.Where(j => !usedJokesIndex.Contains(j.Id)).ToList();

                if (unusedJokes == null || unusedJokes.Count == 0) throw new HttpRequestException("No jokes left", null, HttpStatusCode.NotFound);

                Random rnd = new Random();
                Joke joke = unusedJokes[rnd.Next(0, unusedJokes.Count)];

                usedJokesIndex.Add(joke.Id);

                HttpContext.Session.SetObjectAsJson("UsedJokesIndex", usedJokesIndex);

                return joke;
            }

        }
    }
}
