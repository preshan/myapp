using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PasswordWallet.Core.Contracts;

namespace PasswordWallet.Infrastructure.Security
{
    /// <summary>
    /// AES-256-CBC with per-value IV. Session key from master password (PBKDF2).
    /// </summary>
    public sealed class AesCryptoProvider : ICryptoProvider
    {
        public const string VersionPrefix = "v2:";

        private const int KeySizeBytes = 32;
        private const int IvSizeBytes = 16;
        private const int Pbkdf2Iterations = 100000;

        private byte[] _sessionKey;

        public string CipherPrefix
        {
            get { return VersionPrefix; }
        }

        public bool IsInitialized
        {
            get { return _sessionKey != null && _sessionKey.Length == KeySizeBytes; }
        }

        public void Initialize(string masterPassword, byte[] salt)
        {
            if (string.IsNullOrEmpty(masterPassword))
                throw new ArgumentException("Master password is required.", "masterPassword");
            if (salt == null || salt.Length < 16)
                throw new ArgumentException("Salt must be at least 16 bytes.", "salt");

            using (var derive = new Rfc2898DeriveBytes(masterPassword, salt, Pbkdf2Iterations))
                _sessionKey = derive.GetBytes(KeySizeBytes);
        }

        public void Clear()
        {
            if (_sessionKey != null)
            {
                Array.Clear(_sessionKey, 0, _sessionKey.Length);
                _sessionKey = null;
            }
        }

        public string Encrypt(string plainText)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Unlock the wallet before encrypting.");
            if (plainText == null)
                plainText = string.Empty;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = _sessionKey;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs, Encoding.UTF8))
                        sw.Write(plainText);

                    return VersionPrefix + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Unlock the wallet before decrypting.");
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            if (!cipherText.StartsWith(VersionPrefix, StringComparison.Ordinal))
                throw new CryptographicException("Unsupported cipher format.");

            byte[] payload = Convert.FromBase64String(cipherText.Substring(VersionPrefix.Length));
            if (payload.Length <= IvSizeBytes)
                throw new CryptographicException("Invalid cipher payload.");

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = _sessionKey;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[IvSizeBytes];
                Buffer.BlockCopy(payload, 0, iv, 0, IvSizeBytes);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(payload, IvSizeBytes, payload.Length - IvSizeBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                    return sr.ReadToEnd();
            }
        }

        public string DecryptStoredValue(string storedValue)
        {
            if (string.IsNullOrEmpty(storedValue))
                return string.Empty;

            if (storedValue.StartsWith(VersionPrefix, StringComparison.Ordinal))
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("Unlock the wallet before decrypting stored values.");
                return Decrypt(storedValue);
            }

            return LegacyTripleDesCryptoProvider.Decrypt(storedValue, true);
        }
    }
}
