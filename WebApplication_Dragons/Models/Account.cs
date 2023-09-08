using MongoDB.Entities;
using System.Runtime.Serialization;
using System.Data;

namespace WebApplication_Dragons.Models
{
    [Collection(nameof(Account))]
    public class Account : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] AccountRoles { get; set; }
        public Account()
        {
            //PluralizationService.pluralize
        }
    }
}
