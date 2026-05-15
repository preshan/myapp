using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PasswordWallet.Infrastructure.Configuration
{
    /// <summary>
    /// Resolves database and configuration file paths (app folder, then AppData).
    /// </summary>
    public static class ApplicationPaths
    {
        public const string AppFolderName = "Password Wallet";
        public const string DatabaseFileName = "Database1.mdb";
        public const string ConfigFileName = "wallet.config";

        public static string ApplicationDirectory
        {
            get
            {
                string exePath = Assembly.GetExecutingAssembly().Location;
                if (!string.IsNullOrEmpty(exePath))
                    return Path.GetDirectoryName(exePath);
                return Application.StartupPath;
            }
        }

        public static string AppDataDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    AppFolderName);
            }
        }

        public static string DatabasePath
        {
            get
            {
                string besideExe = Path.Combine(ApplicationDirectory, DatabaseFileName);
                if (File.Exists(besideExe))
                    return besideExe;

                string inAppData = Path.Combine(AppDataDirectory, DatabaseFileName);
                if (File.Exists(inAppData))
                    return inAppData;

                return besideExe;
            }
        }

        public static string ConfigPath
        {
            get { return Path.Combine(AppDataDirectory, ConfigFileName); }
        }

        public static void EnsureAppDataDirectory()
        {
            if (!Directory.Exists(AppDataDirectory))
                Directory.CreateDirectory(AppDataDirectory);
        }

        public static void SeedDatabaseIfMissing()
        {
            if (File.Exists(DatabasePath))
                return;

            EnsureAppDataDirectory();
            string appDataTarget = Path.Combine(AppDataDirectory, DatabaseFileName);

            string[] candidates = new[]
            {
                Path.Combine(ApplicationDirectory, DatabaseFileName),
                Path.Combine(ApplicationDirectory, "..", "data", DatabaseFileName),
                Path.Combine(ApplicationDirectory, "..", "..", "data", DatabaseFileName)
            };

            foreach (string candidate in candidates)
            {
                string fullPath = Path.GetFullPath(candidate);
                if (File.Exists(fullPath))
                {
                    File.Copy(fullPath, appDataTarget, false);
                    return;
                }
            }
        }
    }
}
