using MongoDB.Entities;


namespace WebApplication_Dragons.Models
{
    [Collection("Accounts")]
    public class Account : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
