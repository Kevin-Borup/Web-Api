namespace WebApplication_JokeMachine.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string PunchLine { get; set; }

        public Joke(int id, string category, string content, string punchline)
        {
            this.Id = id;
            this.Category = category;
            this.Content = content;
            this.PunchLine = punchline;
        }
    }
}
