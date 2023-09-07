using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.DTOs
{
    public class RouteKeyDTO
    {
        public string Identifier { get; set; }
        public string Key { get; set; }

        public override string ToString()
        {
            return $"{Identifier}.{Key}";
        }

        public RouteKeyDTO(RouteKey routeKey)
        {
            Identifier = routeKey.Identifier;
            Key = routeKey.Key;
        }
    }
}
