namespace WebApplication_Dragons.DTOs
{
    public class TuneDTO
    {
        public string Name { get; set; }
        public string Duration { get; set; }

        public TuneDTO(string name, string duration)
        {
            this.Name = name;
            this.Duration = duration;
        }
    }
}
