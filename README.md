# Password Wallet

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.5-512BD4?style=flat-square)](https://www.microsoft.com/net/framework)
[![Platform](https://img.shields.io/badge/platform-Windows%20x86-0078D4?style=flat-square)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)

A **Windows desktop password manager** built with C#, .NET Framework 4.5, WinForms, and DevExpress 14.1.

Originally developed **2012–2015** as a personal project (first shipped application). This repository preserves that era’s stack while applying a **layered architecture**, clearer naming, and **modern security** for open-source sharing.

> Suitable as a portfolio sample for classic .NET desktop development (Visual Studio 2012–2015, LOB WinForms, MS Access).

## What it does

- One **master password** protects the vault
- Save **name, URL, username, password, notes** per entry
- Search, edit, delete, copy to clipboard
- Dark DevExpress UI (Visual Studio 2013 theme)

## Application flow

1. **CreateMasterPasswordForm** — first run only  
2. **LoginForm** — master password + optional hint  
3. **VaultForm** — manage stored credentials  

## Tech stack (2015-era)

| Layer | Technology |
|-------|------------|
| UI | WinForms + DevExpress 14.1 |
| Runtime | .NET Framework 4.5 |
| Database | Microsoft Access `.mdb` (Jet OLE DB 4.0) |
| IDE | Visual Studio 2012–2015 |
| Build | MSBuild, **x86** platform |

## Requirements

| Requirement | Notes |
|-------------|--------|
| **Windows** | Jet OLE DB 4.0 (32-bit) |
| **.NET Framework 4.5** | |
| **Visual Studio 2012–2015+** | Open `Password_Wallet.sln` |
| **DevExpress WinForms 14.1** | Required to build ([license](https://www.devexpress.com/)) |
| **Platform** | **x86** (not Any CPU) |

## Quick start

```text
1. Clone the repository
2. Install DevExpress 14.1
3. Build: Debug | x86
4. Add Database1.mdb locally (see below) — never commit your vault
5. Run PasswordWallet.exe
```

### Database (local only — not in this repo)

The vault file **`Database1.mdb`** is **not** included in git (personal data).

| Step | Action |
|------|--------|
| 1 | Use your own empty/template `.mdb`, or extract from original `Password_Wallet.zip` (ZIP password: `1852`) |
| 2 | Place `Database1.mdb` in `data/` or beside `PasswordWallet.exe` |
| 3 | First run may copy it to `%AppData%\Password Wallet\` |

Legacy Jet passwords are detected automatically; settings are stored in `wallet.config` (Windows DPAPI).

## Project structure

Layered architecture (typical 2012–2015 enterprise desktop style):

```
PasswordWallet/
├── Core/              Models + repository interfaces
├── Infrastructure/    Crypto, paths, settings
├── Data/              Access repositories + migration
├── Business/          Services + session context
└── Presentation/      Forms + entry point
```

Full details: **[ARCHITECTURE.md](ARCHITECTURE.md)**

## Security highlights (open-source fork)

| Topic | Implementation |
|-------|----------------|
| Master password | PBKDF2 (100k iterations) |
| Entry passwords | AES-256-CBC, key from master password |
| SQL | Parameterized queries |
| Legacy upgrade | Automatic migration from original TripleDES format |

See **[SECURITY.md](SECURITY.md)** — back up your `.mdb` before upgrading.

## Build

```text
Solution:  Password_Wallet.sln
Project:   PasswordWallet\Password_Wallet.csproj
Config:    Debug | Release
Platform:  x86
```

Clean + Rebuild if DevExpress references fail.

## Why this repo is a fair 2015 portfolio piece

- Real **end-to-end desktop app** (not a tutorial todo list)
- Shows **WinForms + third-party controls**, **ADO.NET**, and **local database** skills common in that period
- Honest about **platform limits** (Windows x86, DevExpress dependency)
- Documented **architecture** and **security** thinking for reviewers

## License

[MIT](LICENSE) — application source. DevExpress components require a separate commercial license.

## Author

**Preshan Pradeepa Kariyawasam**  
[preshanpradeepa@gmail.com](mailto:preshanpradeepa@gmail.com)

Original application (2015). Refactored for open source (2026).
