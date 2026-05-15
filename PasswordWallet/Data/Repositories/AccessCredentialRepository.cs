using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Core.Models;

namespace PasswordWallet.Data.Repositories
{
    internal sealed class AccessCredentialRepository : ICredentialRepository
    {
        private readonly OleDbConnection _connection;
        private readonly ICryptoProvider _crypto;

        public AccessCredentialRepository(OleDbConnection connection, ICryptoProvider crypto)
        {
            _connection = connection;
            _crypto = crypto;
        }

        public IList<string> GetAllNames()
        {
            var names = new List<string>();
            using (var cmd = CreateCommand("SELECT [name] FROM [Table2] ORDER BY [name]"))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    names.Add(reader.GetString(0));
            }
            return names;
        }

        public IList<string> SearchByPrefix(string prefix, CredentialSearchField field)
        {
            if (string.IsNullOrEmpty(prefix))
                return GetAllNames();

            string column;
            switch (field)
            {
                case CredentialSearchField.Url: column = "url"; break;
                case CredentialSearchField.Username: column = "username"; break;
                default: column = "name"; break;
            }

            var names = new List<string>();
            string sql = string.Format(
                "SELECT [name] FROM [Table2] WHERE [{0}] LIKE @prefix ORDER BY [name]", column);

            using (var cmd = CreateCommand(sql))
            {
                cmd.Parameters.AddWithValue("@prefix", prefix + "%");
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        names.Add(reader.GetString(0));
                }
            }
            return names;
        }

        public Credential GetByName(string name)
        {
            using (var cmd = CreateCommand(
                "SELECT [name],[url],[username],[password],[other] FROM [Table2] WHERE [name] = @name"))
            {
                cmd.Parameters.AddWithValue("@name", name);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new Credential
                    {
                        Name = reader.GetString(0),
                        Url = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Username = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Password = _crypto.DecryptStoredValue(
                            reader.IsDBNull(3) ? string.Empty : reader.GetString(3)),
                        Notes = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                    };
                }
            }
        }

        public bool Exists(string name)
        {
            using (var cmd = CreateCommand("SELECT COUNT(*) FROM [Table2] WHERE [name] = @name"))
            {
                cmd.Parameters.AddWithValue("@name", name);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return Convert.ToInt32(reader[0]) > 0;
                }
            }
        }

        public bool IsDuplicate(string name, string username, string encryptedPassword)
        {
            using (var cmd = CreateCommand(
                "SELECT COUNT(*) FROM [Table2] WHERE [name]=@name AND [username]=@username AND [password]=@password"))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", encryptedPassword);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return Convert.ToInt32(reader[0]) > 0;
                }
            }
        }

        public void Insert(Credential credential, string encryptedPassword)
        {
            using (var cmd = CreateCommand(
                "INSERT INTO [Table2] ([name],[url],[username],[password],[other]) " +
                "VALUES (@name,@url,@username,@password,@other)"))
            {
                AddParameters(cmd, credential, encryptedPassword);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(string originalName, Credential credential, string encryptedPassword)
        {
            using (var cmd = CreateCommand(
                "UPDATE [Table2] SET [name]=@name,[url]=@url,[username]=@username," +
                "[password]=@password,[other]=@other WHERE [name]=@originalName"))
            {
                cmd.Parameters.AddWithValue("@originalName", originalName);
                AddParameters(cmd, credential, encryptedPassword);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(string name)
        {
            using (var cmd = CreateCommand("DELETE FROM [Table2] WHERE [name] = @name"))
            {
                cmd.Parameters.AddWithValue("@name", name.Trim());
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAll()
        {
            using (var cmd = CreateCommand("DELETE FROM [Table2]"))
                cmd.ExecuteNonQuery();
        }

        private static void AddParameters(OleDbCommand cmd, Credential credential, string encryptedPassword)
        {
            cmd.Parameters.AddWithValue("@name", credential.Name);
            cmd.Parameters.AddWithValue("@url", credential.Url ?? string.Empty);
            cmd.Parameters.AddWithValue("@username", credential.Username ?? string.Empty);
            cmd.Parameters.AddWithValue("@password", encryptedPassword);
            cmd.Parameters.AddWithValue("@other", credential.Notes ?? string.Empty);
        }

        private OleDbCommand CreateCommand(string sql)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return new OleDbCommand(sql, _connection);
        }
    }
}
