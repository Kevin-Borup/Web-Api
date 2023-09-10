namespace WebApplication_JokeMachine.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string AccessLevel { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string PunchLine { get; set; }

        public Joke(int id, string accessLevel, string language, string category, string content, string punchline)
        {
            this.Id = id;
            this.AccessLevel = accessLevel;
            this.Language = language;
            this.Category = category;
            this.Content = content;
            this.PunchLine = punchline;
        }
    }
}
