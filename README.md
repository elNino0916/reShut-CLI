![image](https://github.com/user-attachments/assets/1040a495-7a6c-4117-8d2e-f8145d599269)


## reShut CLI is an easy-to-use tool that aims to streamline and improve the management of system reboots and shutdowns on your Windows computer.
> [!CAUTION]
> This is an early preview version of reShut CLI. Features and functionality may change before the final release. Use at your own risk, as this version may contain bugs or incomplete features.

> [!WARNING]
> This branch may not be updated regularly because I don't have much time and I test out changes locally first before pushing them eventually.
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

### Remote management

Execute actions on another machine via command-line:

```
reShutCLI.exe -remote:HOST -user:USERNAME -pass:PASSWORD -s
reShutCLI.exe -remote:HOST -user:USERNAME -pass:PASSWORD -r
```

### Advanced scheduling

When scheduling, you can now enter minutes, phrases like `in 2 hours`, or explicit dates such as `2025-12-31 23:59`. After confirmation, choose to repeat the action daily or weekly.
