using WebApplication_JokeMachine.Models;

namespace WebApplication_JokeMachine.Interfaces
{
    public interface IEncryptionService
    {
        string GenerateIdentifier();
        ApiKey GenerateKey(string identifier, string identifierSalt, out string apiKey);

        string HashIdentifier(string identifier, string identifierSalt);
        bool CheckKey(string apiKey, string storedKey, string storedKeySalt);

    }
}
