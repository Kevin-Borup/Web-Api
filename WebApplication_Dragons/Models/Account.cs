using MongoDB.Entities;
using System.Runtime.Serialization;
using System.Data;
using WebApplication_Dragons.Services;

namespace WebApplication_Dragons.Models
{
    [Collection("Accounts")]
    public class Account : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PassSalt { get; set; }
        public string[] AccountRoles { get; set; }
    }
}
