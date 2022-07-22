using shufflecad_4.Holders;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace shufflecad_4.Helpers
{
    public class CommandsHelper
    {
        private static Task taskChecks;
        private static bool stopChecks;

        private static Process StandartRun(string commandPath)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                string winSCPDir = "/C " + Directory.GetCurrentDirectory() + "/WinSCP/WinSCP.com";
                string commandDir = Directory.GetCurrentDirectory() + commandPath;
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd", winSCPDir + " /script=" + commandDir);
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.StandardOutputEncoding = Encoding.GetEncoding(850);
                process.StartInfo = startInfo;
                process.Start();
                return process;
            }
            catch (Exception e)
            {
                InfoHolder.UserLogger.LogWrite(e.ToString());
            }
            return null;
        }

        private static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        public static void StartChecks()
        {
            stopChecks = false;
            taskChecks = Task.Run(() =>
            {
                while (!stopChecks)
                {
                    try
                    {
                        bool ric = PingHost(InfoHolder.CurrentSettings.IpAddress);
                        if (ric)
                        {
                            InfoHolder.CurrentRPIData.RIC = true;
                            Process pirProcess = StandartRun("/PyCommands/CheckIsRunning.txt");
                            if (pirProcess != null)
                            {
                                pirProcess.WaitForExit();
                                string result = pirProcess.StandardOutput.ReadToEnd();
                                int res = result.IndexOf(InfoHolder.CurrentSettings.MainFileName);
                                if (res == -1)
                                {
                                    InfoHolder.CurrentRPIData.PIR = false;
                                    InfoHolder.CurrentRPIData.PIC = false;
                                }
                                else
                                {
                                    InfoHolder.CurrentRPIData.PIR = true;
                                }
                            }
                        }
                        else
                        {
                            InfoHolder.CurrentRPIData.RIC = false;
                            InfoHolder.CurrentRPIData.PIC = false;
                            InfoHolder.CurrentRPIData.PIR = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        InfoHolder.UserLogger.LogWrite(ex.ToString());
                    }
                    Thread.Sleep(200);
                }
            });
        }

        async public static Task StopChecks()
        {
            stopChecks = true;
            await taskChecks;
        }

        async public static Task StartProgram()
        {
            // берем текущее окно
            MainWindow mainWindow = MainWindow.ThisMainWindow;

            bool check = string.IsNullOrEmpty(InfoHolder.CurrentSettings.PathToSrc) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.MainFileName) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.IpAddress) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.Password) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.UserName);
            if (check)
            {
                mainWindow.ChangeStateText("Not all fields are filled up!", MainWindow.STATE_ERROR_COLOR);
            }
            else
            {
                if (InfoHolder.CurrentRPIData.RIC)
                {
                    mainWindow.ChangeStateText("Loading Commands...");
                    await Task.Run(() =>
                    {
                        StandartRun("/PyCommands/LoadCommands.txt").WaitForExit();
                    });
                    mainWindow.ChangeStateText("Deploying...");
                    await Task.Run(() =>
                    {
                        StandartRun("/PyCommands/StopProgram.txt").WaitForExit();
                        StandartRun("/PyCommands/StartProgram.txt").WaitForExit();
                    });
                    mainWindow.ChangeStateText("Deployed");
                }
                else
                {
                    mainWindow.ChangeStateText("Robot is not connected", MainWindow.STATE_WARNING_COLOR);
                }
            }
        }

        async public static Task StopProgram()
        {
            // берем текущее окно
            MainWindow mainWindow = MainWindow.ThisMainWindow;

            bool check = string.IsNullOrEmpty(InfoHolder.CurrentSettings.PathToSrc) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.MainFileName) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.IpAddress) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.Password) ||
                string.IsNullOrEmpty(InfoHolder.CurrentSettings.UserName);
            if (check)
            {
                mainWindow.ChangeStateText("Not all fields are filled up!", MainWindow.STATE_ERROR_COLOR);
            }
            else
            {
                if (InfoHolder.CurrentRPIData.RIC)
                {
                    mainWindow.ChangeStateText("Stopping...");
                    await Task.Run(() =>
                    {
                        StandartRun("/PyCommands/StopProgram.txt").WaitForExit();
                    });
                    mainWindow.ChangeStateText("Stopped");
                }
                else
                {
                    mainWindow.ChangeStateText("Robot is not connected", MainWindow.STATE_WARNING_COLOR);
                }
            }
        }
    }
}
