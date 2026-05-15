using System;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Core.Models;
using PasswordWallet.Data;

namespace PasswordWallet.Business.Services
{
    public sealed class CredentialService
    {
        private readonly AccessWalletUnitOfWork _unitOfWork;

        public CredentialService(AccessWalletUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ICredentialRepository Repository
        {
            get { return _unitOfWork.Credentials; }
        }

        private ICryptoProvider Crypto
        {
            get { return _unitOfWork.Crypto; }
        }

        public void Add(Credential credential)
        {
            ValidateRequiredFields(credential);

            string encrypted = Crypto.Encrypt(credential.Password ?? string.Empty);
            if (Repository.Exists(credential.Name))
            {
                if (Repository.IsDuplicate(credential.Name, credential.Username, encrypted))
                    throw new InvalidOperationException(
                        "A record with this name, username, and password already exists.");
                throw new InvalidOperationException("A record with this name already exists.");
            }

            Repository.Insert(credential, encrypted);
        }

        public void Update(string originalName, Credential credential)
        {
            ValidateRequiredFields(credential);

            string encrypted = Crypto.Encrypt(credential.Password ?? string.Empty);
            if (credential.Name != originalName)
            {
                if (Repository.IsDuplicate(credential.Name, credential.Username, encrypted))
                    throw new InvalidOperationException(
                        "A record with this name, username, and password already exists.");
                if (Repository.Exists(credential.Name))
                    throw new InvalidOperationException("A record with this name already exists.");
            }

            Repository.Update(originalName, credential, encrypted);
        }

        public void Delete(string name)
        {
            Repository.Delete(name);
        }

        public void DeleteAll()
        {
            Repository.DeleteAll();
        }

        private static void ValidateRequiredFields(Credential credential)
        {
            if (credential == null || string.IsNullOrWhiteSpace(credential.Name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(credential.Username))
                throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(credential.Password))
                throw new ArgumentException("Password is required.");
        }
    }
}
