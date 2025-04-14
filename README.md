# External EXE launcher for VNyan
Can be used to run PowerShell scripts, external EXEs, PowerShell commands and CMD.EXE commands

## WARNING: 
Passing user input, such as Twitch chat, to external commands is dangerous! I will not be held responsible if a user sends e.g. ```'; RMDIR /S C:\Windows\System32``` and you didn't handle that correctly!  
You are responsible for [sanitising your own inputs](https://xkcd.com/327/) if you decide to do this!  
For Powershell, ensure you use single quotes around your parameters ```'``` never use double quotes ```"``` and replace all single quotes in any user input with two single quotes ```''``` For other EXEs, personal scripts etc. you should write your scripts accordingly.  
If you do not understand the above. Do not connect data or parameters from any of VNyan's "Callback" nodes to this plugin. Someone will ruin your day!  
![image](https://github.com/user-attachments/assets/3e18f990-32e3-419d-b0d4-17f88ab5f747)

## WARNING: Third-party node graphs
You should always be cautious when importing a node-graph made by someone else (and plugins for that matter!). Ensure you understand exactly what it is doing before you activate it. However if you have this plugin installed you need to be even more cautious. Someone could hide a malicious command in a node graph they supply, and now it has the ability to run anything on your system. If you don't know what you're doing, don't import third party node graphs, or better yet, don't install this plugin!  

## Functions

```_lum_exe_ps5``` - Run a script under PowerShell 5  
text1 - Full path to script  
text2 - Parameters to pass to script  
text3 - Trigger to callback  
num1 - 0=hide window (default), 1=show window  

Callback:  
text1 - Full text output of script
num1 - Exit code from script

```_lum_exe_pwsh``` - As above, but uses PowerShell Core  
```_lum_exe_run``` - As above, but runs an executable (e.g. Notepad.exe) instead of a script  

```_lum_exe_ps5cmd``` - Run a raw powershell command  
text1 - Command to run  
text3 - Trigger to callback  
num1 - 0=hide window (default), 1=show window  

```_lum_exe_pwshcmd``` - As above, but uses PowerShell Core  
```_lum_exe_cmd``` - As above, but uses CMD.EXE for running a dos-style command  
