using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordWallet.Infrastructure.Configuration
{
    /// <summary>
    /// Per-machine settings. Persisted with Windows DPAPI (not committed to source control).
    /// </summary>
    public sealed class WalletSettings
    {
        private const char Separator = '|';

        public string JetDatabasePassword { get; set; }

        public byte[] EncryptionSalt { get; set; }

        public static WalletSettings Load()
        {
            ApplicationPaths.EnsureAppDataDirectory();
            if (!File.Exists(ApplicationPaths.ConfigPath))
                return CreateDefault();

            try
            {
                string[] parts = File.ReadAllText(ApplicationPaths.ConfigPath).Split(Separator);
                if (parts.Length < 2)
                    return CreateDefault();

                return new WalletSettings
                {
                    JetDatabasePassword = Unprotect(parts[0]),
                    EncryptionSalt = Convert.FromBase64String(parts[1])
                };
            }
            catch
            {
                return CreateDefault();
            }
        }

        public void Save()
        {
            ApplicationPaths.EnsureAppDataDirectory();
            string content = Protect(JetDatabasePassword) + Separator +
                             Convert.ToBase64String(EncryptionSalt);
            File.WriteAllText(ApplicationPaths.ConfigPath, content);
        }

        private static WalletSettings CreateDefault()
        {
            var settings = new WalletSettings
            {
                JetDatabasePassword = string.Empty,
                EncryptionSalt = new byte[32]
            };
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(settings.EncryptionSalt);
            settings.Save();
            return settings;
        }

        private static string Protect(string plain)
        {
            if (string.IsNullOrEmpty(plain))
                return string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(plain);
            byte[] protectedBytes = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        private static string Unprotect(string stored)
        {
            if (string.IsNullOrEmpty(stored))
                return string.Empty;
            byte[] data = ProtectedData.Unprotect(
                Convert.FromBase64String(stored), null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(data);
        }
    }
}
