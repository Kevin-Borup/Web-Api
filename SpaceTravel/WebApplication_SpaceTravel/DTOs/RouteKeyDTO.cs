using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.DTOs
{
    // A DTO object to guarantee we only send data we should.
    public class RouteKeyDTO
    {
        public string Identifier { get; set; }
        public string Key { get; set; }

        public override string ToString()
        {
            return $"{Identifier}.{Key}";
        }

        public RouteKeyDTO(string identifier, string key)
        {
            Identifier = identifier;
            Key = key;
        }
    }
}
