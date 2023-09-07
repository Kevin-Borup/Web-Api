using WebApplication_SpaceTravel.Models;
using WebApplication_SpaceTravel.Services;

namespace TestProject_SpaceTravel
{
    public class EncryptorTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData("fkmaqåøpowdjpaokwdpow")]
        [InlineData("4564654621546")]
        public void KeyValidation(string identifierSalt)
        {
            EncryptorService encryptor = new EncryptorService();

            string iden = encryptor.GenerateIdentifier();

            RouteKey key = encryptor.GenerateKey(iden, identifierSalt, out string newkey);

            Assert.True(encryptor.CheckKey(newkey, key.Key, key.KeySalt));

        }
    }
}