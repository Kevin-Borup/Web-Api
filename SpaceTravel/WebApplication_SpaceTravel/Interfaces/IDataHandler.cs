using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Interfaces
{
    public interface IDataHandler
    {
        Task<IEnumerable<GalacticRoute>> GetAll();
        Task<IEnumerable<GalacticRoute>> GetWithinYear();

        Task InsertRouteKey(RouteKey key);
        Task UpdateKeyQueryData(RouteKey key);
        Task<RouteKey?> GetKeyIfIdentifierExists(string identifier);
    }
}
