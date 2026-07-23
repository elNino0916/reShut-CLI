
<img width="1102" height="140" alt="image" src="https://github.com/user-attachments/assets/46180d26-19d4-43b1-a3f5-fa6eea5eeecf" />


## reShut CLI is an easy-to-use tool that aims to streamline and improve the management of system reboots and shutdowns on Windows computers in your local network.

> [!NOTE]
> reShut CLI 2.1 runs on .NET 10. The installer automatically downloads and installs the .NET 10 runtime if it is missing — no manual setup required.

-----
New in v2.1.0
* Modernized codebase (.NET 10, nullable reference types, source-generated P/Invoke and JSON)
* New NSIS installer that installs all dependencies automatically and migrates from the old installer
* Fixed: "Auto update on startup" setting was not being honored
* Fixed: skipping the confirmation prompt no longer waits for an extra key press
* Faster startup and lower memory footprint (no WinForms dependency anymore)
-----
New Features in v2.0.0
* Translations
* Bugfixes and Performance Improvements
* Updating without Reset
* New Theme
* Dynamic Default Theme
* Advanced scheduling with specific dates, recurring intervals, or natural-language timing
* Remote management to trigger shutdown or reboot on authenticated remote hosts
-----

### Remote management: Usage

Execute actions on another machine via command-line:

```
reShutCLI.exe -remote:HOST -user:USERNAME -pass:PASSWORD -s
reShutCLI.exe -remote:HOST -user:USERNAME -pass:PASSWORD -r
```

### Building

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```
dotnet build reShutCLI.sln -c Release
```

### Building the installer

Requires [NSIS 3.x](https://nsis.sourceforge.io/) in addition to the .NET 10 SDK:

```powershell
.\installer\build-installer.ps1
```

This publishes the app (framework-dependent, win-x64) and produces `installer\reShutCLI-2.1.0-setup.exe`. The installer:
* automatically downloads and installs the .NET 10 runtime when missing,
* silently uninstalls a previous Inno Setup based installation before upgrading,
* supports fully silent installs via `/S`.
