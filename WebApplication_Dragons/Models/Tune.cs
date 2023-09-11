using MongoDB.Entities;
using WebApplication_Dragons.DTOs;

namespace WebApplication_Dragons.Models
{
    [Collection("Tunes")]
    public class Tune : Entity
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }

        public Tune(TuneDTO tunedto, int index)
        {
            this.Index = index;
            this.Name = tunedto.Name;
            this.Duration = tunedto.Duration;
        }
    }
}
