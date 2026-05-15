using System;
using PasswordWallet.Infrastructure.Configuration;

namespace PasswordWallet.Core.Contracts
{
    /// <summary>
    /// Single database connection scope for one application session (Unit of Work).
    /// </summary>
    public interface IWalletUnitOfWork : IDisposable
    {
        ICredentialRepository Credentials { get; }

        IMasterPasswordRepository MasterPassword { get; }

        WalletSettings Settings { get; }

        void MigrateLegacyData(string masterPasswordPlaintext);
    }
}
