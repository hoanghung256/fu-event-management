using System.Security.Cryptography;

namespace FUEM.Infrastructure.Common
{
    public class Hasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public static bool Verify(string input, string hashedInput)
        {
            var parts = hashedInput.Split(':');
            if (parts.Length != 2)
                throw new FormatException("The hash is not in the correct format.");
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] actualHash = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
