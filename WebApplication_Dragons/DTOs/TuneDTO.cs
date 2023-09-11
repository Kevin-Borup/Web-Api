using WebApplication_Dragons.Models;

namespace WebApplication_Dragons.DTOs
{
    public class TuneDTO
    {
        public string Name { get; set; }
        public string Duration { get; set; }

        public TuneDTO(Tune tune)
        {
            this.Name = tune.Name;
            this.Duration = tune.Duration;
        }
    }
}
