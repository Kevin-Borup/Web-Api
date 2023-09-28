using MongoDB.Entities;

namespace WebApplication_Dragons.Models
{
    [Collection("Tunes")]
    public class Tune : Entity
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }

        public Tune(int index, string name, string duration)
        {
            this.Index = index;
            this.Name = name;
            this.Duration = duration;
        }
    }
}
