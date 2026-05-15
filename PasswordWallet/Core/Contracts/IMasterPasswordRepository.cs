namespace PasswordWallet.Core.Contracts
{
    public interface IMasterPasswordRepository
    {
        bool IsConfigured();

        string GetHint();

        bool Validate(string password);

        void Create(string passwordHash, string encryptedHint);

        void Update(string passwordHash, string encryptedHint);
    }
}
