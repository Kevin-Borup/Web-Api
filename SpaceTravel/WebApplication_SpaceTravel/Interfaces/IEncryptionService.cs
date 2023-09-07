using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Interfaces
{
    public interface IEncryptionService
    {
        string GenerateIdentifier();
        RouteKey GenerateKey(string identifier, string identifierSalt, out string apiKey);

        string HashIdentifier(string identifier, string identifierSalt);
        bool CheckKey(string apiKey, string storedKey, string storedKeySalt);

    }
}
