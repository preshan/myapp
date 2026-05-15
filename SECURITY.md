# Security

## Supported versions

| Version | Supported |
|---------|-----------|
| 2.3 (current source on GitHub) | Yes |
| Original 2015 installer / older binaries | No |

## Report a problem

Please do **not** post exploit details in a public GitHub issue.

Email: **preshanpradeepa@gmail.com** with:

- What you found
- Steps to reproduce
- Affected version / commit

## Scope

Password Wallet is a **local Windows desktop** program. It is not a synced or cloud vault.

- Protect your PC user account and use disk encryption.
- Anyone with a copy of `Database1.mdb` and the Jet password can open the file outside the app.
- Decrypted data may exist in memory while the vault is unlocked.
- You need your own DevExpress and .NET Framework patches on the machine you build or run on.

## Upgrading an old database

Back up `Database1.mdb` before running this build on a file from the original 2015 app.

On first successful login, the program may:

1. Store the master password as a PBKDF2 hash (instead of the old reversible format)
2. Encrypt the password hint
3. Re-encrypt saved passwords with AES (values prefixed with `v2:` in the database)

Legacy encryption in the source exists only so those old files can be read once and migrated.
