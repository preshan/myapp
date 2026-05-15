using PasswordWallet.Business.Services;
using PasswordWallet.Data;

namespace PasswordWallet.Business
{
    /// <summary>
    /// Application-wide session after master password unlock (2012-style app context).
    /// </summary>
    public sealed class WalletApplicationContext
    {
        private static WalletApplicationContext _current;

        private WalletApplicationContext(
            AccessWalletUnitOfWork unitOfWork,
            CredentialService credentialService,
            MasterPasswordService masterPasswordService)
        {
            UnitOfWork = unitOfWork;
            Credentials = credentialService;
            MasterPassword = masterPasswordService;
        }

        public static WalletApplicationContext Current
        {
            get { return _current; }
        }

        public AccessWalletUnitOfWork UnitOfWork { get; private set; }

        public CredentialService Credentials { get; private set; }

        public MasterPasswordService MasterPassword { get; private set; }

        public bool IsUnlocked
        {
            get { return UnitOfWork.Crypto.IsInitialized; }
        }

        public static WalletApplicationContext StartNew()
        {
            End();
            var unitOfWork = new AccessWalletUnitOfWork();
            _current = new WalletApplicationContext(
                unitOfWork,
                new CredentialService(unitOfWork),
                new MasterPasswordService(unitOfWork));
            return _current;
        }

        public void Unlock(string masterPassword)
        {
            UnitOfWork.Unlock(masterPassword);
            UnitOfWork.MigrateLegacyData(masterPassword);
        }

        public static void End()
        {
            if (_current != null)
            {
                _current.UnitOfWork.Dispose();
                _current = null;
            }
        }
    }
}
