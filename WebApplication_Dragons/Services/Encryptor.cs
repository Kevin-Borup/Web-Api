using System.Security.Cryptography;
using System.Text;

namespace WebApplication_Dragons.Services
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
        public void EncryptPassword(string password, out byte[] pass, out byte[] salt)
        {
            byte[] hashSalt = GenerateSalt(); // This salt is used to further convolute the stored values.
            pass = HashPassword(Encoding.UTF8.GetBytes(password), hashSalt, encryptionIterations);
            salt = hashSalt;
        }
        private byte[] GenerateSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[byteLength]; // Antallet af bytes til saltet
            randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }

        private byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            // Rfc2898DeriveBytes salts the byte password, followed by hashing it the defined amount of times, with the selected SHA512
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAlgorithmName.SHA512))
            {
                return rfc2898.GetBytes(byteLength);
            }
        }

        /// <summary>
        /// Check if the password ends with the same hash value, as the one stored on the database.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public bool CheckPassword(string password, byte[] pass, byte[] salt)
        {
            bool equal = false;
            // Hash the input to compare with the one on the database.
            byte[] newPass = HashPassword(Encoding.UTF8.GetBytes(password), salt, encryptionIterations);

            if (pass.Length == newPass.Length)
            {
                // If they are same length, and each element of the byte array is equal, then it's the same hash.
                equal = CompareByteArrays(pass, newPass, newPass.Length);
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
