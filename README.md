# debug_failure

## Getting Started

* Run `docker-compose up` to get the database running
* Run `dotnet ef database update --context ApplicationDataContext` from `src/` to push the identity tables into the DB
* Run `dotnet ef database update --context DataProtectionKeysContext` from `src/` to push the data protection tables into the DB

## Reproducing the debugger crash

* Open the root directory in Visual Studio Code
* Place a breakpoint in `src/Controllers/HomeController.cs:28` on the `return View(user);` line in the `Index()` method.
* Run the application using the debugger (using the .NET Core Launch (web) task).
* The application will hit the breakpoint once the browser has launched.
* Mouse over the `user` variable or attempt to expand it in the Variables section in the debug window. This will crash the debugger.
