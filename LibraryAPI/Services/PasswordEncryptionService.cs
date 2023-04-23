using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace LibraryAPI.Services
{
    public sealed class PasswordEncryptionService : IPasswordEncryptionService
    {
        private const int Iteration = 50000;
        private const int KeySize = 32;

        public string HashPassword(string password, byte[] salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iteration,
                HashAlgorithmName.SHA512,
                KeySize);

            return Convert.ToBase64String(hash);
        }

        public byte[] GenerateSalt() =>
            RandomNumberGenerator.GetBytes(32);

        public bool IsPasswordEqual(string password, string hash, byte[] salt)
        {
            var regeneratedHash = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteration, HashAlgorithmName.SHA512, KeySize));

            return regeneratedHash.Equals(hash);
        }

    }
}
