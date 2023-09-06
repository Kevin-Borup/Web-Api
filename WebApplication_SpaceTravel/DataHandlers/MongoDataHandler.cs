using MongoDB.Entities;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.DataHandlers
{
    public class MongoDataHandler : IDataHandlers
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
            var routesWithinYear = new List<GalacticRoute>();

            var allRoutes = await DB.Find<GalacticRoute>().ManyAsync(_ => true);
            
            var routeDurations = new List<TimeSpan>();
            foreach (var route in allRoutes)
            {
                string duration = route.Duration.ToLower();
                if (duration.Contains("dag") || duration.Contains("dage"))
                {
                    if (int.TryParse(duration, out int days)) routeDurations.Add(TimeSpan.FromDays(days));
                }
                else if (duration.Contains("month") || duration.Contains("months"))
                {
                    if (int.TryParse(duration, out int months)) routeDurations.Add(TimeSpan.FromDays(months * 30.44)); // average length of months in gregorian, with leap years accounted for

                }
                else if(duration.Contains("year") || duration.Contains("years"))
                {
                    if (int.TryParse(duration, out int years)) routeDurations.Add(TimeSpan.FromDays(years * 365.2425)); //.25 to account for leap years
                }
            }


            return routesWithinYear;
        }

    }
}
