# External EXE launcher for VNyan
Can be used to run PowerShell scripts, external EXEs, PowerShell commands and CMD.EXE commands

## WARNING: Read this first
Passing user input, such as the text parameter in Twitch redeems to external EXEs is dangerous! I will not be held responsible if a user sends e.g. '; RMDIR /S C:\Windows\System32  
If you choose to do this, you are responsible for [sanitising your own inputs](https://xkcd.com/327/)
For Powershell, ensure you use single quotes around your parameters ' never use double quotes " and replace all quotes in any user input with '' For other EXEs, personal scripts etc. you should write your scripts accordingly.
If you have this plugin installed, and you import a node graph supplied by someone else. Be extra cautious about what that graph may contain!

![image](https://github.com/user-attachments/assets/3e18f990-32e3-419d-b0d4-17f88ab5f747)

## Functions

```_lum_exe_ps5``` - Run a script under PowerShell 5  
text1 - Full path to script  
text2 - Parameters to pass to script  
text3 - Trigger to callback  
num1 - 0=hide window, 1=show window  

Callback:  
text1 - Full text output of script
num1 - Exit code from script

```_lum_exe_pwsh``` - As above, but uses PowerShell Core
```_lum_exe_run``` - As above, but runs an executable (e.g. Notepad.exe) instead of a script
```_lum_exe_ps5cmd``` - Run a raw powershell command  
text1 - Command to run  
text3 - Trigger to callback  
num1 - 0=hide window, 1=show window  

```_lum_exe_pwshcmd``` - As above, but uses PowerShell Core  
```_lum_exe_cmd``` - As above, but uses CMD.EXE for running a dos-style command  
Same parameters as above
