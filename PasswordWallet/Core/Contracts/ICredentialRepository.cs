using System.Collections.Generic;
using PasswordWallet.Core.Models;

namespace PasswordWallet.Core.Contracts
{
    public interface ICredentialRepository
    {
        IList<string> GetAllNames();

        IList<string> SearchByPrefix(string prefix, CredentialSearchField field);

        Credential GetByName(string name);

        bool Exists(string name);

        bool IsDuplicate(string name, string username, string encryptedPassword);

        void Insert(Credential credential, string encryptedPassword);

        void Update(string originalName, Credential credential, string encryptedPassword);

        void Delete(string name);

        void DeleteAll();
    }
}
