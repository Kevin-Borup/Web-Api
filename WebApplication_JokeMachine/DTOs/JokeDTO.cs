namespace WebApplication_JokeMachine.DTOs
{
    public class JokeDTO
    {
        public string Content { get; set; }
        public string PunchLine { get; set; }

        public JokeDTO(string content, string punchline)
        {
            this.Content = content;
            this.PunchLine = punchline;
        }
    }
}
