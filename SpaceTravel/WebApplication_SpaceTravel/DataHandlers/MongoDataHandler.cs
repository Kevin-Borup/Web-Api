using MongoDB.Entities;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.DataHandlers
{
    public class MongoDataHandler : IDataHandler
    {
        public MongoDataHandler()
        {
            DB.InitAsync("ScrumBoard", "localhost", 32768);
        }

        public async Task<IEnumerable<GalacticRoute>> GetAll()
        {
            return await DB.Find<GalacticRoute>().ManyAsync(_ => true);
        }

        public async Task<IEnumerable<GalacticRoute>> GetWithinYear()
        {
            return await DB.Find<GalacticRoute>().ManyAsync(q => !q.Duration.ToLower().Contains("year"));
        }

        public async Task InsertRouteKey(RouteKey key)
        {
            await DB.InsertAsync<RouteKey>(key);
        }

        public async Task UpdateKeyQueryData(RouteKey key)
        {
            DB.Update<RouteKey>().MatchID(key.ID).ModifyWith(key);
        }

        public async Task<RouteKey?> GetKeyIfIdentifierExists(string identifier)
        {
            var matchingKey = await DB.Find<RouteKey>().ManyAsync(q => q.Identifier.Equals(identifier));
            var routeKey = matchingKey.FirstOrDefault();
            return routeKey;
        }
    }
}
