using MongoDB.Entities;

namespace WebApplication_SpaceTravel.Models
{
    [Collection("GalacticRoutes")]
    public class GalacticRoute : Entity
    {
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string[] NavigationPoints { get; set; }
        public string Duration { get; set; }
        public string[] Dangers { get; set; }
        public string FuelUsage { get; set; }
        public string Description { get; set; }
    }
}
