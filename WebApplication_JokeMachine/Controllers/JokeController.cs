using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication_JokeMachine.DataHandlers;
using WebApplication_JokeMachine.DTOs;
using WebApplication_JokeMachine.Extenders;
using WebApplication_JokeMachine.Models;


namespace WebApplication_JokeMachine.Controllers
{
    [Route("/")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        LocalDataHandler _dataHandler;

        public JokeController()
        {
            _dataHandler = new LocalDataHandler();
        }

        [HttpGet("GetJoke")]
        public async Task<JokeDTO> GetJoke([FromQuery] string category = "")
        {
            string? lang = Request.Headers.ContentLanguage;

            if (lang == null) throw new HttpRequestException("No language specified", null, HttpStatusCode.NotFound);

            if (HttpContext.Session.IsAvailable)
            {
                string? access = HttpContext.Session.GetString("SessionAccess");

                if (access == null) throw new HttpRequestException("Access not certain", null, HttpStatusCode.Unauthorized);

                List<Joke> allJokes = _dataHandler.GetJokes(access, lang, category);

                if (allJokes == null || allJokes.Count == 0) throw new HttpRequestException("No jokes", null, HttpStatusCode.NotFound);

                var usedJokesIndex = HttpContext.Session.GetObjectFromJson<List<int>>("UsedJokesIndex");
                if (usedJokesIndex == null) usedJokesIndex= new List<int>();

                var unusedJokes = allJokes.Where(j => !usedJokesIndex.Contains(j.Id)).ToList();

                if (unusedJokes == null || unusedJokes.Count == 0) throw new HttpRequestException("No jokes left", null, HttpStatusCode.NotFound);

                Random rnd = new Random();
                Joke joke = unusedJokes[rnd.Next(0, unusedJokes.Count)];

                usedJokesIndex.Add(joke.Id);

                HttpContext.Session.SetObjectAsJson("UsedJokesIndex", usedJokesIndex);

                return new JokeDTO(joke.Content, joke.PunchLine);
            }

            throw new HttpRequestException("No jokes left", null, HttpStatusCode.NotFound);
        }
    }
}
