using System;
using System.Data;
using System.Data.OleDb;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Infrastructure.Security;

namespace PasswordWallet.Data.Repositories
{
    internal sealed class AccessMasterPasswordRepository : IMasterPasswordRepository
    {
        private readonly OleDbConnection _connection;
        private readonly ICryptoProvider _crypto;

        public AccessMasterPasswordRepository(OleDbConnection connection, ICryptoProvider crypto)
        {
            _connection = connection;
            _crypto = crypto;
        }

        public bool IsConfigured()
        {
            using (var cmd = CreateCommand("SELECT COUNT(*) FROM [Table]"))
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                return Convert.ToInt32(reader[0]) > 0;
            }
        }

        public string GetHint()
        {
            using (var cmd = CreateCommand("SELECT hint FROM [Table]"))
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read() || reader.IsDBNull(0))
                    return string.Empty;

                string hint = reader.GetString(0);
                if (!hint.StartsWith(AesCryptoProvider.VersionPrefix, StringComparison.Ordinal))
                    return hint;

                if (!_crypto.IsInitialized)
                    return "(Hint is encrypted — log in to view or change it in settings)";

                return _crypto.Decrypt(hint);
            }
        }

        public bool Validate(string password)
        {
            using (var cmd = CreateCommand("SELECT password FROM [Table]"))
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    return false;
                return PasswordHasher.Verify(password, reader.GetString(0));
            }
        }

        public void Create(string passwordHash, string encryptedHint)
        {
            using (var cmd = CreateCommand(
                "INSERT INTO [Table] ([password], [hint]) VALUES (@password, @hint)"))
            {
                cmd.Parameters.AddWithValue("@password", passwordHash);
                cmd.Parameters.AddWithValue("@hint", encryptedHint);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(string passwordHash, string encryptedHint)
        {
            using (var cmd = CreateCommand(
                "UPDATE [Table] SET [password] = @password, [hint] = @hint"))
            {
                cmd.Parameters.AddWithValue("@password", passwordHash);
                cmd.Parameters.AddWithValue("@hint", encryptedHint);
                cmd.ExecuteNonQuery();
            }
        }

        private OleDbCommand CreateCommand(string sql)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return new OleDbCommand(sql, _connection);
        }
    }
}
