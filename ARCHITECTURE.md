# Architecture

WinForms UI calls into business services, which use data repositories against a Jet/Access `.mdb` file. Shared types and interfaces live in `Core`; crypto and paths are in `Infrastructure`.

## Layers

| Folder | Purpose |
|--------|---------|
| `Presentation` | `Program.cs`, `LoginForm`, `VaultForm`, `CreateMasterPasswordForm`, `AboutForm` |
| `Business` | `WalletApplicationContext`, `CredentialService`, `MasterPasswordService` |
| `Data` | `AccessWalletUnitOfWork`, repositories, legacy migration |
| `Infrastructure` | `ApplicationPaths`, `WalletSettings`, `AesCryptoProvider`, `PasswordHasher` |
| `Core` | `Credential` model, repository/crypto interfaces |

## Startup

1. `Program.Main` creates `WalletApplicationContext` and opens the database.
2. If no master password row exists → `CreateMasterPasswordForm`.
3. Otherwise → `LoginForm` → on success → `VaultForm`.
4. Session crypto key is set when the master password validates; credential passwords are encrypted with that key.

## Database

- `[Table]` — master password hash and hint  
- `[Table2]` — stored credentials (`other` column maps to `Credential.Notes` in code)

## Build

Target **x86**, .NET 4.5, DevExpress 14.1 referenced from the GAC or install path.
