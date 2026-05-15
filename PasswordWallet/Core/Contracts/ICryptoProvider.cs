namespace PasswordWallet.Core.Contracts
{
    /// <summary>
    /// Encrypts and decrypts vault field values using the unlocked session key.
    /// </summary>
    public interface ICryptoProvider
    {
        string CipherPrefix { get; }

        bool IsInitialized { get; }

        void Initialize(string masterPassword, byte[] salt);

        void Clear();

        string Encrypt(string plainText);

        string Decrypt(string cipherText);

        string DecryptStoredValue(string storedValue);
    }
}
