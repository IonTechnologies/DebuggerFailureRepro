# debug_failure

## Getting Started

* First, run the docker containers to get the database.
* Second, run `dotnet ef database update --context ApplicationDataContext` from `src/` to push the identity tables into the DB
* Third, run `dotnet ef database update --context DataProtectionKeysContext` from `src/` to push the data protection tables into the DB
* Forth, run the project with `dotnet watch`.
* Fifth, profit.
