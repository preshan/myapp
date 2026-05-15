using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using PasswordWallet.Infrastructure.Configuration;

namespace PasswordWallet.Data.Access
{
    internal static class AccessConnectionFactory
    {
        private static readonly string[] LegacyJetPasswords = { "101010@123", "1852", string.Empty };

        public static OleDbConnection Open(WalletSettings settings)
        {
            string path = ApplicationPaths.DatabasePath;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    "Database not found. Place Database1.mdb beside the application or in %AppData%\\Password Wallet.",
                    path);
            }

            var passwordsToTry = new List<string>();
            if (!string.IsNullOrEmpty(settings.JetDatabasePassword))
                passwordsToTry.Add(settings.JetDatabasePassword);

            foreach (string legacy in LegacyJetPasswords)
            {
                if (!passwordsToTry.Contains(legacy))
                    passwordsToTry.Add(legacy);
            }

            Exception lastError = null;
            foreach (string jetPassword in passwordsToTry)
            {
                try
                {
                    var builder = new OleDbConnectionStringBuilder
                    {
                        Provider = "Microsoft.Jet.OLEDB.4.0",
                        DataSource = path
                    };
                    if (!string.IsNullOrEmpty(jetPassword))
                        builder["Jet OLEDB:Database Password"] = jetPassword;

                    var connection = new OleDbConnection(builder.ConnectionString);
                    connection.Open();

                    if (settings.JetDatabasePassword != jetPassword)
                    {
                        settings.JetDatabasePassword = jetPassword;
                        settings.Save();
                    }

                    return connection;
                }
                catch (Exception ex)
                {
                    lastError = ex;
                }
            }

            throw new InvalidOperationException(
                "Could not open the Access database. Verify Database1.mdb and its Jet password.",
                lastError);
        }
    }
}
