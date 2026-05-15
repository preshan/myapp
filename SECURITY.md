# Security Policy

## Supported versions

| Version | Supported |
|---------|-----------|
| 2.3+ (layered architecture fork) | Yes |
| Original 2012–2015 release | No — upgrade path via automatic migration on login |

## Reporting a vulnerability

If you discover a security issue, please **do not** open a public GitHub issue with exploit details.  
Contact the maintainer privately with:

- Description of the issue
- Steps to reproduce
- Impact assessment

## Known limitations

This is a **local, single-user** desktop vault, not a cloud password manager.

- Data is only as safe as your Windows user account and disk encryption.
- Jet/Access `.mdb` files can be copied offline; use full-disk encryption.
- DevExpress and .NET Framework dependencies must be kept patched on the host OS.
- Memory may contain decrypted passwords while the vault is unlocked.
- Legacy databases are upgraded on first successful login; keep a backup before upgrading.

## Migration

On first login after upgrading, the app will:

1. Re-hash the master password with PBKDF2
2. Encrypt the hint with AES-256
3. Re-encrypt all `Table2` passwords from legacy TripleDES to `v2:` AES

Back up `Database1.mdb` before running the upgraded build.
