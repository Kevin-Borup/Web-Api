using Amazon.Runtime.SharedInterfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebApplication_SpaceTravel.Exceptions;

namespace WebApplication_SpaceTravel.Models
{
    public class Encryptor
    {
        private int byteLength = 32;
        private int encryptionIterations = 5000;

        /// <summary>
        /// Encrypts the password with SHA512 and salt, returns the new pass and used salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        public RouteKey GenerateKey(string title)
        {
            RouteKey routeKey = new RouteKey();


            var idenTitle = GenerateUniqueIdentifier(title);

            var keyPass = GenerateUniqueKey();
            var keySalt = GenerateSalt();
            var keyHash = HashBytes(keyPass, keySalt, encryptionIterations);

            routeKey.Identifier = Convert.ToBase64String(idenTitle);
            routeKey.Key = Convert.ToBase64String(keyPass);

            return routeKey;
        }
        private byte[] GenerateSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[byteLength]; // Antallet af bytes til saltet
            randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }

        private byte[] GenerateUniqueIdentifier(string title)
        {
            if (title.Equals("Captain"))
            {
                return GenerateUniqueBytes("ScTa4Cad");
            }
            else if (title.Equals("Cadet"))
            {
                return GenerateUniqueBytes("ScTa4Cpt");
            }
            else
            {
                throw new TitleException(title);
            }
        }

        private byte[] GenerateUniqueKey()
        {
            return GenerateUniqueBytes();
        }

        private byte[] GenerateUniqueBytes(string charValues = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890") 
        {
            char[] chars = charValues.ToCharArray();
            byte[] data = GenerateSalt();
            StringBuilder result = new StringBuilder(byteLength);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return Encoding.UTF8.GetBytes(result.ToString());
        }

        private byte[] HashBytes(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            // Rfc2898DeriveBytes salts the byte password, followed by hashing it the defined amount of times, with the selected SHA512
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAlgorithmName.SHA512))
            {
                return rfc2898.GetBytes(byteLength);
            }
        }

        public string HashIdentifier(string identifer, string identifierSalt)
        {
            return Convert.ToBase64String(HashBytes(Encoding.UTF8.GetBytes(identifer), Encoding.UTF8.GetBytes(identifierSalt), encryptionIterations));
        }

        /// <summary>
        /// Check if the password ends with the same hash value, as the one stored on the database.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="storedKey"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        public bool CheckKey(string apiKey, string storedKey, string storedSalt)
        {
            bool equal = false;

            byte[] key = Encoding.UTF8.GetBytes(storedKey);

            // Hash the input to compare with the one on the database.
            byte[] newPass = HashBytes(Encoding.UTF8.GetBytes(apiKey), Encoding.UTF8.GetBytes(storedSalt), encryptionIterations);

            if (key.Length == newPass.Length)
            {
                // If they are same length, and each element of the byte array is equal, then it's the same hash.
                equal = CompareByteArrays(key, newPass, newPass.Length);
            }

            return equal;
        }

        private bool CompareByteArrays(byte[] a, byte[] b, int len)
        {
            for (int i = 0; i < len; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
    }
}
