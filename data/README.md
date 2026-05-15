# Database folder

Place `Database1.mdb` here for **first-run seeding** when the app cannot find a database elsewhere.

## Obtaining the file

1. Extract `Password_Wallet.zip` from the original distribution (ZIP password: `1852`).
2. Copy `Database1.mdb` into this folder.

The application copies it to `%AppData%\Password Wallet\` or uses it beside the executable if configured.

## Git

**`Database1.mdb` is gitignored** because it contains encrypted credentials.  
Do not commit your personal vault to a public repository.

For open-source demos, ship an empty template database separately or document how users create a new vault on first run.
