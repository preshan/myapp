using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordWallet.Infrastructure.Security
{
    /// <summary>
    /// Original release cipher (TripleDES + fixed key). Read-only for migration.
    /// </summary>
    internal static class LegacyTripleDesCryptoProvider
    {
        private const string LegacyKey = "axo@!1L3";

        public static string Encrypt(string plainText, bool useHashing)
        {
            byte[] keyArray;
            byte[] input = Encoding.UTF8.GetBytes(plainText);

            if (useHashing)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                    keyArray = md5.ComputeHash(Encoding.UTF8.GetBytes(LegacyKey));
            }
            else
                keyArray = Encoding.UTF8.GetBytes(LegacyKey);

            using (var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                byte[] result = tdes.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                return Convert.ToBase64String(result);
            }
        }

        public static string Decrypt(string cipherText, bool useHashing)
        {
            byte[] input = Convert.FromBase64String(cipherText);
            byte[] keyArray;

            if (useHashing)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                    keyArray = md5.ComputeHash(Encoding.UTF8.GetBytes(LegacyKey));
            }
            else
                keyArray = Encoding.UTF8.GetBytes(LegacyKey);

            using (var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                byte[] result = tdes.CreateDecryptor().TransformFinalBlock(input, 0, input.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
    }
}
