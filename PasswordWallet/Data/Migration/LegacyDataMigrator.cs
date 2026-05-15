using System;
using System.Data.OleDb;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Infrastructure.Security;

namespace PasswordWallet.Data.Migration
{
    internal static class LegacyDataMigrator
    {
        public static void Migrate(OleDbConnection connection, ICryptoProvider crypto, string masterPasswordPlaintext)
        {
            if (!crypto.IsInitialized)
                return;

            MigrateMasterRecord(connection, crypto, masterPasswordPlaintext);
            MigrateCredentials(connection, crypto);
        }

        private static void MigrateMasterRecord(
            OleDbConnection connection, ICryptoProvider crypto, string masterPasswordPlaintext)
        {
            using (var select = new OleDbCommand("SELECT password, hint FROM [Table]", connection))
            using (var reader = select.ExecuteReader())
            {
                if (!reader.Read())
                    return;

                string password = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                string hint = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                reader.Close();

                bool changed = false;
                string newPassword = password;
                string newHint = hint;

                if (!password.StartsWith(PasswordHasher.HashPrefix, StringComparison.Ordinal))
                {
                    if (!PasswordHasher.Verify(masterPasswordPlaintext, password))
                        return;
                    newPassword = PasswordHasher.Hash(masterPasswordPlaintext);
                    changed = true;
                }

                if (!string.IsNullOrEmpty(hint) &&
                    !hint.StartsWith(AesCryptoProvider.VersionPrefix, StringComparison.Ordinal))
                {
                    newHint = crypto.Encrypt(hint);
                    changed = true;
                }

                if (!changed)
                    return;

                using (var update = new OleDbCommand(
                    "UPDATE [Table] SET [password]=@password, [hint]=@hint", connection))
                {
                    update.Parameters.AddWithValue("@password", newPassword);
                    update.Parameters.AddWithValue("@hint", newHint);
                    update.ExecuteNonQuery();
                }
            }
        }

        private static void MigrateCredentials(OleDbConnection connection, ICryptoProvider crypto)
        {
            using (var select = new OleDbCommand("SELECT [name], [password] FROM [Table2]", connection))
            using (var reader = select.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string stored = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);

                    if (string.IsNullOrEmpty(stored) ||
                        stored.StartsWith(AesCryptoProvider.VersionPrefix, StringComparison.Ordinal))
                        continue;

                    string plain = LegacyTripleDesCryptoProvider.Decrypt(stored, true);
                    string encrypted = crypto.Encrypt(plain);

                    using (var update = new OleDbCommand(
                        "UPDATE [Table2] SET [password]=@password WHERE [name]=@name", connection))
                    {
                        update.Parameters.AddWithValue("@password", encrypted);
                        update.Parameters.AddWithValue("@name", name);
                        update.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
