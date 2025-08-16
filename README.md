> [!CAUTION]
> **Important Notice (from v2.0.0):**
> The installer imports a **self-signed code-signing certificate** (EKU: Code Signing, OID 1.3.6.1.5.5.7.3.3).
> - Installing a self-signed certificate into the **Root store** can be flagged by antivirus as **potentially malicious**, since a trusted root could, in theory, be abused.
>
> So, if you encounter any issues, disable your antivirus temporarily while installing reShutCLI. You can enable it again afterwards.


<img width="966" height="110" alt="image" src="https://github.com/user-attachments/assets/50ad8948-8efe-4535-90f8-1a0ab4fc9273" />

## reShut CLI is an easy-to-use tool that aims to streamline and improve the management of system reboots and shutdowns on your Windows computer.

> [!WARNING]
> .NET 9 is now required. Download it [here](https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/9.0.8/windowsdesktop-runtime-9.0.8-win-x64.exe).

-----
New Features in v2.0.0
* Translations
* Bugfixes and Performance Improvements
* Updating without Reset
* New Theme
* .NET 9
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
