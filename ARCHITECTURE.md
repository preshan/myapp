# Password Wallet — Architecture

Classic **layered architecture** for a .NET Framework 4.5 WinForms desktop app (Visual Studio 2012–2015 style): Presentation → Business → Data → Infrastructure, with shared **Core** contracts and models.

## Layer diagram

```
┌─────────────────────────────────────────────────────────┐
│  Presentation (WinForms + DevExpress)                   │
│  Program, LoginForm, VaultForm, CreateMasterPasswordForm│
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│  Business                                               │
│  WalletApplicationContext, CredentialService,           │
│  MasterPasswordService                                  │
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│  Data                                                   │
│  AccessWalletUnitOfWork, Repositories, LegacyDataMigrator│
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│  Infrastructure                                         │
│  ApplicationPaths, WalletSettings, AesCryptoProvider,   │
│  PasswordHasher, LegacyTripleDesCryptoProvider          │
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│  Core                                                   │
│  Credential model, ICredentialRepository, ICryptoProvider│
└─────────────────────────────────────────────────────────┘
```

## Responsibilities

| Layer | Namespace | Role |
|-------|-----------|------|
| **Core** | `PasswordWallet.Core.*` | Domain model (`Credential`), interfaces — no UI or database code |
| **Infrastructure** | `PasswordWallet.Infrastructure.*` | Paths, DPAPI settings, cryptography |
| **Data** | `PasswordWallet.Data.*` | Jet OLE DB access, repositories, legacy migration |
| **Business** | `PasswordWallet.Business.*` | Validation rules, session context, orchestration |
| **Presentation** | `PasswordWallet.Presentation.*` | Forms and application entry point |

## Key types (2012 naming conventions)

| Type | Purpose |
|------|---------|
| `WalletApplicationContext` | App-wide session after unlock (static `Current`) |
| `AccessWalletUnitOfWork` | Single DB connection + repositories (Unit of Work) |
| `ICredentialRepository` | CRUD for `Table2` |
| `IMasterPasswordRepository` | Master row in `Table` |
| `ICryptoProvider` | Encrypt/decrypt with session key |
| `CredentialService` | Business rules for duplicate detection |
| `MasterPasswordService` | First-run setup, login validation, password change |

## Data flow — login

1. `Program.Main` → `WalletApplicationContext.StartNew()`
2. `LoginForm` → `MasterPasswordService.Validate`
3. `WalletApplicationContext.Unlock` → PBKDF2 session key + `LegacyDataMigrator`
4. `VaultForm` uses `CredentialService` / repositories through `Current`

## Database

- **Engine:** Microsoft Access `.mdb` via `Microsoft.Jet.OLEDB.4.0` (32-bit / x86)
- **Tables:** `[Table]` (master), `[Table2]` (credentials)
- **Location:** Beside `PasswordWallet.exe`, then `%AppData%\Password Wallet\`
- **Secrets:** Jet password stored in `wallet.config` (DPAPI), never in source control

## Compatibility

| Item | Version |
|------|---------|
| .NET Framework | 4.5 |
| Visual Studio | 2012, 2013, 2015+ |
| Platform target | **x86** (required for Jet) |
| DevExpress WinForms | 14.1 |

## Design choices (era-appropriate)

- **No DI container** — explicit construction in `WalletApplicationContext` (typical 2012 LOB apps)
- **Interfaces in Core** — enables testing and clear layer boundaries without over-engineering
- **Unit of Work** — one `OleDbConnection` per session
- **Repository per aggregate** — credentials vs master password
