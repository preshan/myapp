using System;
using PasswordWallet.Data;
using PasswordWallet.Infrastructure.Security;

namespace PasswordWallet.Business.Services
{
    public sealed class MasterPasswordService
    {
        private readonly AccessWalletUnitOfWork _unitOfWork;

        public MasterPasswordService(AccessWalletUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsFirstRun()
        {
            return !_unitOfWork.MasterPassword.IsConfigured();
        }

        public string GetHint()
        {
            return _unitOfWork.MasterPassword.GetHint();
        }

        public bool Validate(string password)
        {
            return _unitOfWork.MasterPassword.Validate(password);
        }

        public void Create(string password, string hint)
        {
            _unitOfWork.Crypto.Initialize(password, _unitOfWork.Settings.EncryptionSalt);
            string hash = PasswordHasher.Hash(password);
            string encryptedHint = _unitOfWork.Crypto.Encrypt(hint ?? string.Empty);
            _unitOfWork.MasterPassword.Create(hash, encryptedHint);
            _unitOfWork.Crypto.Clear();
        }

        public void Change(string currentPassword, string newPassword, string hint)
        {
            if (!_unitOfWork.MasterPassword.Validate(currentPassword))
                throw new InvalidOperationException("Current master password is incorrect.");

            _unitOfWork.MasterPassword.Update(
                PasswordHasher.Hash(newPassword),
                _unitOfWork.Crypto.Encrypt(hint ?? string.Empty));
        }
    }
}
