namespace WebApplication_JokeMachine.DTOs
{
    public class ApiKeyDTO
    {
        public string Identifier { get; set; }
        public string Key { get; set; }

        public ApiKeyDTO(string identifier, string key)
        {
            this.Identifier = identifier;
            this.Key = key;
        }

        public override string ToString()
        {
            return Identifier + "." + Key;
        }
    }
}
