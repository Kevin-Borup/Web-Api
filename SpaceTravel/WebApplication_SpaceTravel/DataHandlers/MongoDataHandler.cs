using MongoDB.Entities;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.DataHandlers
{
    public class MongoDataHandler : IDataHandler
    {
        public MongoDataHandler()
        {
            DB.InitAsync("SpaceTravel", "localhost", 32768);
        }

        public async Task<IEnumerable<GalacticRoute>> GetAll()
        {
            return await DB.Find<GalacticRoute>().ManyAsync(_ => true);
        }

        /// <summary>
        /// Get all routes that finish within a year
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GalacticRoute>> GetWithinYear()
        {
            return await DB.Find<GalacticRoute>().ManyAsync(q => !q.Duration.ToLower().Contains("år"));
        }

        public async Task InsertRouteKey(RouteKey key)
        {
            await DB.InsertAsync<RouteKey>(key);
        }

        public async Task UpdateKeyQueryData(RouteKey key)
        {
            await DB.Update<RouteKey>().MatchID(key.ID).ModifyWith(key).ExecuteAsync();
        }

        /// <summary>
        /// Gets the first key that has the same identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public async Task<RouteKey?> GetKeyIfIdentifierExists(string identifier)
        {
            var matchingKey = await DB.Find<RouteKey>().ManyAsync(q => q.Identifier.Equals(identifier));
            if (matchingKey == null) return null;
            if (matchingKey.Count() == 0) return null;

            var routeKey = matchingKey.First();
            return routeKey;
        }
    }
}
