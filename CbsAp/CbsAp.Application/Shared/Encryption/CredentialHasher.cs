using System.Security.Cryptography;
using System.Text;

namespace CbsAp.Application.Shared.Encryption
{
    public class CredentialHasher : IHasher
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string storedHash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, hashAlgorithm, keySize);

            bool isMatch = CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(storedHash));
            return isMatch;
        }
    }
}