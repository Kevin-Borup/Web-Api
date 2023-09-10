namespace WebApplication_JokeMachine.Models
{
    public class ApiKey
    {
        public string AccessLevel { get; set; }
        public string Identifier { get; set; }
        public string Key { get; set; }
        public string KeySalt { get; set; }
    }
}
