using WebApplication_JokeMachine.Models;

namespace WebApplication_JokeMachine.Interfaces
{
    public interface IDataHandler
    {
        List<Joke> GetJokes(string access, string language = "da", string category = "");

        void InsertApiKey(ApiKey key);
        ApiKey GetKeyIfIdentifierExists(string identifier);
    }
}
