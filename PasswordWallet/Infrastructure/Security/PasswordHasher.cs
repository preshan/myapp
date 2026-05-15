using System;
using System.Security.Cryptography;

namespace PasswordWallet.Infrastructure.Security
{
    public static class PasswordHasher
    {
        public const string HashPrefix = "pbkdf2:";

        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            byte[] hash;
            using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations))
                hash = derive.GetBytes(HashSize);

            return HashPrefix + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            if (storedHash.StartsWith(HashPrefix, StringComparison.Ordinal))
                return VerifyPbkdf2(password, storedHash);

            return LegacyTripleDesCryptoProvider.Encrypt(password, true) == storedHash;
        }

        private static bool VerifyPbkdf2(string password, string storedHash)
        {
            try
            {
                string payload = storedHash.Substring(HashPrefix.Length);
                string[] parts = payload.Split(':');
                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expected = Convert.FromBase64String(parts[1]);

                byte[] actual;
                using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations))
                    actual = derive.GetBytes(HashSize);

                return FixedTimeEquals(actual, expected);
            }
            catch
            {
                return false;
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
