# Password Wallet

Small Windows desktop app to store website logins locally. Built around **2012–2015** with C#, .NET Framework 4.5, WinForms, and DevExpress 14.1.

Repository: [github.com/preshan/myapp](https://github.com/preshan/myapp)

## Features

- Master password on startup
- Save name, URL, username, password, and notes per entry
- Search, edit, delete entries
- Copy username/password to clipboard

## Requirements

- Windows (32-bit Jet OLE DB for `.mdb` files)
- .NET Framework 4.5
- Visual Studio 2012 or later
- DevExpress WinForms 14.1 (needed to compile)
- Build platform: **x86**

## Build

1. Open `Password_Wallet.sln`
2. Restore/reference DevExpress 14.1 assemblies
3. Set configuration to **Debug** or **Release**, platform **x86**
4. Build solution

Output: `PasswordWallet\bin\x86\Debug\PasswordWallet.exe` (path may vary by configuration)

## Database

The Access database `Database1.mdb` is **not** in this repo.

Put your own file in:

- `data\Database1.mdb`, or  
- next to `PasswordWallet.exe`, or  
- `%AppData%\Password Wallet\`

If you have the original zip distribution from 2015, use that archive to obtain `Database1.mdb` (see old release notes for the zip password).

## Project layout

```
PasswordWallet/
  Presentation/   Forms and Program.cs
  Business/       Application logic
  Data/           Access database code
  Infrastructure/ Encryption and settings
  Core/           Models and interfaces
```

More detail in [ARCHITECTURE.md](ARCHITECTURE.md).

## Notes (2026 update)

This source tree was cleaned up for GitHub: clearer folder names, parameterized SQL, and stronger encryption than the original 2015 build. Existing `.mdb` files are migrated on first login after upgrading.

See [SECURITY.md](SECURITY.md) before using an old database file.

## License

MIT — see [LICENSE](LICENSE). DevExpress UI controls need their own license.

## Author

Preshan Pradeepa Kariyawasam — preshanpradeepa@gmail.com
