using System;
using UnityEngine;
using VNyanInterface;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using UnityEngine.Networking.Types;
using static System.Net.Mime.MediaTypeNames;

namespace VNyan_exe
{
    public class VNyan_exe : MonoBehaviour, VNyanInterface.ITriggerHandler {
        string cmdPath;
        string ps5Path;
        string ps7Path;
        void CallVNyan(string TriggerName, int int1, int int2, int int3, string Text1, string Text2, string Text3) {
            if (TriggerName.Length > 0) {
                VNyanInterface.VNyanInterface.VNyanTrigger.callTrigger(TriggerName, int1, int2, int3, Text1, Text2, Text3);
            } else {
                VNyanInterface.VNyanInterface.VNyanTrigger.callTrigger("_lum_miu_error", 0, 0, 0, "Invalid trigger name", "", "");
            }
        }
        float GetVNyanDecimal(string DecimalName) {
            return VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterFloat(DecimalName);
        }
        string GetVNyanText(string TextParamName) {
            return VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterString(TextParamName);
        }
        void Log(string Message) {
            CallVNyan("_lum_dbg_raw", 0, 0, 0, Message, "", "");
        }
        void ErrorHandler(Exception e) {
            CallVNyan("_lum_dbg_err", 0, 0, 0, "VNyan-EXE", e.ToString(), "");
        }
        public static string FindExePath(string exe) {
            if (!File.Exists(exe)) {
                if (Path.GetDirectoryName(exe) == String.Empty) {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';')) {
                        string path = test.Trim();
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                            return Path.GetFullPath(path);
                    }
                }
                return null;
                //throw new FileNotFoundException(new FileNotFoundException().Message, exe);
            }
            return Path.GetFullPath(exe);
        }

        public void Awake() {
            try {
                VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);
                cmdPath = FindExePath("cmd.exe");
                ps5Path = FindExePath("powershell.exe");
                ps7Path = FindExePath("pwsh.exe");

            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        async Task RunExe(string FileName, string Parameters, string Callback, bool ShowWindow, ProcessWindowStyle WindowStyle) {
            try {
                Log("Running EXE : " + FileName + " " + Parameters+" | "+"Show window : " + ShowWindow.ToString() + " | " + "Window Style: " + WindowStyle.ToString());
                Process EXE = new Process();
                EXE.StartInfo.FileName = FileName;
                EXE.StartInfo.Arguments = Parameters;
                EXE.StartInfo.UseShellExecute = false;
                EXE.StartInfo.RedirectStandardInput = false;
                EXE.StartInfo.RedirectStandardOutput = true;
                EXE.StartInfo.RedirectStandardError = false;
                EXE.StartInfo.CreateNoWindow = !ShowWindow;
                EXE.StartInfo.WindowStyle = WindowStyle;
                EXE.EnableRaisingEvents = true;
                //Log("Start process");
                EXE.Start();
                //Log("Wait for exit");
                EXE.WaitForExit();
                //Log("Connect to stdout");
                StreamReader EXEResult = EXE.StandardOutput;
                //Log("Read stdout");
                string Result = EXEResult.ReadToEnd().TrimEnd();
                //Log("Output: " + Result);
                int ExitCode = EXE.ExitCode;
                //Log("Exit code: " + ExitCode.ToString());
                CallVNyan(Callback, ExitCode, 0, 0, Result, "", "");
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        public void triggerCalled(string name, int int1, int int2, int int3, string text1, string text2, string text3) {
            try {
                if (name.Length > 8) { // _lum_exe_psrun
                    // Log("Trigger: " + name);
                    if (name.Substring(0, 8) == "_lum_exe") {
                        bool ShowWindow;
                        if (int1 > 0) { ShowWindow = true; } else { ShowWindow = false; }
                        // Log("Checking: " + name.Substring(8));
                        switch (name.Substring(8).ToLower()) {
                            case "_run":
                                if (System.IO.File.Exists(text1)) {
                                    Task.Run(() => RunExe(text1, text2, text3, ShowWindow, (ProcessWindowStyle)int2));
                                } else {
                                    Log ("ERR: Failed to run EXE " + text1 + "File not found");
                                }
                                break;

                            case "_ps5":
                                if (ps5Path != null) {
                                    if (System.IO.File.Exists(text1)) {
                                        Task.Run(() => RunExe("powershell.exe", "-NoLogo -File \"" + text1 + "\" " + text2, text3, ShowWindow, (ProcessWindowStyle)int2));
                                    } else {
                                        Log("ERR: Failed to run Windows Powershell script " + text1 + "File not found");
                                    }
                                } else {
                                    Log("Failed to detect Windows Powershell (powershell.exe) at startup. Please fix this and restart VNyan");
                                }
                                break;

                            case "_pwsh":
                                if (ps7Path != null) {
                                    if (System.IO.File.Exists(text1)) {
                                        Task.Run(() => RunExe("pwsh.exe", "-NoLogo -File \"" + text1 + "\" " + text2, text3, ShowWindow, (ProcessWindowStyle)int2));
                                    } else {
                                        Log("ERR: Failed to run Powershell Core script " + text1 + "File not found");
                                    }
                                } else {
                                    Log("Failed to detect Powershell Core (pwsh.exe) at startup. Please fix this and restart VNyan");
                                }
                                break;

                            case "_ps5cmd":
                                if (ps5Path != null) {
                                    Task.Run(() => RunExe("powershell.exe", "-NoLogo -Command \"" + text1 + "\"", text3, ShowWindow, (ProcessWindowStyle)int2));
                                } else {
                                    Log("Failed to detect Windows Powershell (powershell.exe) at startup. Please fix this and restart VNyan");
                                }
                                break;

                            case "_pwshcmd":
                                if (ps7Path != null) {
                                    Task.Run(() => RunExe("powershell.exe", "-NoLogo -Command \"" + text1 + "\"", text3, ShowWindow, (ProcessWindowStyle)int2));
                                } else {
                                    Log("Failed to detect Powershell Core (pwsh.exe) at startup. Please fix this and restart VNyan");
                                }
                                break;

                            case "_cmd":
                                if (cmdPath != null) {
                                    Task.Run(() => RunExe("cmd.exe", "/c " + text1, text3, ShowWindow, (ProcessWindowStyle)int2));
                                } else {
                                    Log("Failed to detect cmd.exe at startup (Oh dear). Please fix this and restart VNyan");
                                }
                                break;
                        }
                    }
                }
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }
    }
}
