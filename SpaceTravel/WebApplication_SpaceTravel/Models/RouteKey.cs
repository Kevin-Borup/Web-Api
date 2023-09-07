using MongoDB.Entities;

namespace WebApplication_SpaceTravel.Models
{
    [Collection("RouteKeys")]
    public class RouteKey : Entity
    {
        public string Title { get; set; }
        public string Identifier { get; set; }
        public string KeySalt { get; set; }
        public string Key { get; set; }
        public DateTime FirstQuery { get; set; }
        public int QueryCount { get; set; }
    }
}
