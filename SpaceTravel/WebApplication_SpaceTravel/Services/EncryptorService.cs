using Amazon.Runtime.SharedInterfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebApplication_SpaceTravel.Exceptions;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Models;

namespace WebApplication_SpaceTravel.Services
{
    public class EncryptorService : IEncryptionService
    {
        private int byteLength = 32;
        private int encryptionIterations = 5000;

        /// <summary>
        /// Generate an identifier, as prefix to the api key.
        /// </summary>
        /// <returns></returns>
        public string GenerateIdentifier()
        {
            return Convert.ToBase64String(GenerateUniqueBytes());
        }

        /// <summary>
        /// Encrypts the password with SHA512 and salt, a RouteKey object and the pure api key.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        public RouteKey GenerateKey(string identifier, string identifierSalt, out string apiKey)
        {

            var identifierBytes = Encoding.UTF8.GetBytes(identifier);
            var identifierSaltBytes = Encoding.UTF8.GetBytes(identifierSalt);
            var idenHash = HashBytes(identifierBytes, identifierSaltBytes, encryptionIterations);

            var apiKeyBytes = GenerateUniqueBytes();
            var keySalt = GenerateSalt();
            var keyHash = HashBytes(apiKeyBytes, keySalt, encryptionIterations);

            RouteKey routeKey = new RouteKey()
            {
                Identifier = Convert.ToBase64String(idenHash),
                Key = Convert.ToBase64String(keyHash),
                KeySalt = Convert.ToBase64String(keySalt),
                FirstQuery = DateTime.Now,
                QueryCount = 0,
            };

            apiKey = Convert.ToBase64String(apiKeyBytes);

            return routeKey;
        }

        /// <summary>
        /// Generate a random salt, used to randomize the algorithm further.
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[byteLength]; // Antallet af bytes til saltet
            randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }

        /// <summary>
        /// Generate a random assortment of bytes, using the alphanumerical alphabet, and a salt.
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateUniqueBytes()
        {
            string charValues = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = charValues.ToCharArray();
            byte[] data = GenerateSalt();
            StringBuilder result = new StringBuilder(byteLength);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return Encoding.UTF8.GetBytes(result.ToString());
        }

        /// <summary>
        /// Hashing algorithm, that uses SHA512, and specified iterations.
        /// </summary>
        /// <param name="toBeHashed"></param>
        /// <param name="salt"></param>
        /// <param name="numberOfRounds"></param>
        /// <returns></returns>
        private byte[] HashBytes(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            // Rfc2898DeriveBytes salts the byte password, followed by hashing it the defined amount of times, with the selected SHA512
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAlgorithmName.SHA512))
            {
                return rfc2898.GetBytes(byteLength);
            }
        }

        /// <summary>
        /// Hash the identifier with the received salt.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="identifierSalt"></param>
        /// <returns></returns>
        public string HashIdentifier(string identifier, string identifierSalt)
        {
            return Convert.ToBase64String(HashBytes(Encoding.UTF8.GetBytes(identifier), Encoding.UTF8.GetBytes(identifierSalt), encryptionIterations));
        }

        /// <summary>
        /// Check if the password ends with the same hash value, as the one stored on the database.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="storedKey"></param>
        /// <param name="storedKeySalt"></param>
        /// <returns></returns>
        public bool CheckKey(string apiKey, string storedKey, string storedKeySalt)
        {
            bool equal = false;

            byte[] key = Convert.FromBase64String(storedKey);
            byte[] salt = Convert.FromBase64String(storedKeySalt);
            byte[] userKey = Convert.FromBase64String(apiKey);

            // Hash the input to compare with the one on the database.
            byte[] newKey = HashBytes(userKey, salt, encryptionIterations);
            string test = Convert.ToBase64String(newKey);

            if (key.Length == newKey.Length)
            {
                // If they are same length, and each element of the byte array is equal, then it's the same hash.
                equal = CompareByteArrays(key, newKey, newKey.Length);
            }

            return equal;
        }

        /// <summary>
        /// Campares the two byte arrays, to see if each element is identical
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private bool CompareByteArrays(byte[] a, byte[] b, int len)
        {
            for (int i = 0; i < len; i++)
                if (!a[i].Equals(b[i])) return false;
            return true;
        }
    }
}
