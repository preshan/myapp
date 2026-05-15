using System;
using System.Data.OleDb;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Data.Access;
using PasswordWallet.Data.Migration;
using PasswordWallet.Data.Repositories;
using PasswordWallet.Infrastructure.Configuration;
using PasswordWallet.Infrastructure.Security;

namespace PasswordWallet.Data
{
    public sealed class AccessWalletUnitOfWork : IWalletUnitOfWork
    {
        private readonly OleDbConnection _connection;
        private readonly AesCryptoProvider _crypto;

        public AccessWalletUnitOfWork()
        {
            ApplicationPaths.SeedDatabaseIfMissing();
            Settings = WalletSettings.Load();
            _crypto = new AesCryptoProvider();
            _connection = AccessConnectionFactory.Open(Settings);
            Credentials = new AccessCredentialRepository(_connection, _crypto);
            MasterPassword = new AccessMasterPasswordRepository(_connection, _crypto);
        }

        public ICredentialRepository Credentials { get; private set; }

        public IMasterPasswordRepository MasterPassword { get; private set; }

        public WalletSettings Settings { get; private set; }

        public ICryptoProvider Crypto
        {
            get { return _crypto; }
        }

        public void Unlock(string masterPassword)
        {
            _crypto.Initialize(masterPassword, Settings.EncryptionSalt);
        }

        public void Lock()
        {
            _crypto.Clear();
        }

        public void MigrateLegacyData(string masterPasswordPlaintext)
        {
            LegacyDataMigrator.Migrate(_connection, _crypto, masterPasswordPlaintext);
        }

        public void Dispose()
        {
            _crypto.Clear();
            if (_connection != null)
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                    _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
