using shufflecad_4.Classes;
using shufflecad_4.Classes.Variables;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shufflecad_4.Helpers
{
    public class ConnectionHelper
    {
        private static ListenPort inVariablesChannel;
        private static TalkPort outVariablesChannel;
        private static ListenPort chartsChannel;
        private static ListenPort outcadChannel;
        private static ListenPort rpiDataChannel;
        private static ListenPort cameraChannel;

        private static Task helperTask;
        private static bool stopTask;
        private static bool isConnected;

        private static DateTime lastRPIDataUpdate = DateTime.Now;
        public static event EventHandler<EventArgs> OnRPIDataChange;

        public static void StartHelper()
        {
            stopTask = false;
            isConnected = false;

            helperTask = Task.Run(() =>
            {
                while (!stopTask)
                {
                    try
                    {
                        if (InfoHolder.CurrentRPIData.PIR)
                        {
                            if (DateTime.Now - lastRPIDataUpdate > TimeSpan.FromSeconds(3))
                            {
                                InfoHolder.CurrentRPIData.PIC = false;
                            }
                            else
                            {
                                InfoHolder.CurrentRPIData.PIC = true;
                            }

                            if (!isConnected)
                            {
                                SetUpChannels();
                                LinkEvents();
                                StartChannels();

                                isConnected = true;
                            }
                            else
                            {
                                if (!CheckAlive())
                                {
                                    StopChannesls();
                                    InfoHolder.ClearAll();
                                    SetUpChannels();
                                    LinkEvents();
                                    StartChannels();
                                }
                            }
                        }
                        else
                        {
                            if (isConnected)
                            {
                                StopChannesls();
                                InfoHolder.ClearAll();

                                isConnected = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        InfoHolder.UserLogger.LogWrite(ex.ToString());
                    }

                    Thread.Sleep(4);
                }                
            });
        }

        private static void OnInVarsChanged(object sender, EventArgs args)
        {
            try
            {
                // in variables
                string inVarsString = inVariablesChannel.OutString;
                if (inVarsString != "null" && inVarsString != string.Empty)
                {
                    string[] vars = inVarsString.Split('&');
                    foreach (var v in vars)
                    {
                        string[] paramss = v.Split(';');
                        var sv = new ShuffleVariable()
                        {
                            Name = paramss[0],
                            Value = paramss[1],
                            Type = paramss[2],
                            Direction = paramss[3]
                        };
                        if (sv.Direction == ShuffleVariable.IN_DIR)
                        {
                            var contains = InfoHolder.OutVariables.FirstOrDefault(x => x.Name == sv.Name);
                            if (contains == null)
                            {
                                InfoHolder.OutVariables.Add(sv);
                                InfoHolder.OnOutVariablesChangeWrapper();
                            }
                        }
                        else
                        {
                            var contains = InfoHolder.InVariables.FirstOrDefault(x => x.Name == sv.Name);
                            if (contains == null)
                            {
                                InfoHolder.InVariables.Add(sv);
                                InfoHolder.OnInVariablesChangeWrapper();
                            }
                            else
                            {
                                contains.Value = sv.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void OnOutVarsNeedToBeSet(object sender, EventArgs args)
        {
            try
            {
                // out variables
                if (InfoHolder.OutVariables.Count > 0)
                {
                    List<string> stringVars = InfoHolder.OutVariables.Select(x => string.Format("{0};{1}", x.Name, x.Value)).ToList();
                    string sendString = string.Join("&", stringVars);
                    outVariablesChannel.OutString = sendString;
                }
                else
                {
                    outVariablesChannel.OutString = "null";
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void OnChartVarsChanged(object sender, EventArgs args)
        {
            try
            {
                // chart variables
                string chartVarsString = inVariablesChannel.OutString;
                if (chartVarsString != "null" && chartVarsString != string.Empty)
                {
                    string[] vars = chartVarsString.Split('&');
                    foreach (var v in vars)
                    {
                        string[] paramss = v.Split(';');
                        var cv = new ChartVariable()
                        {
                            Name = paramss[0],
                            Value = paramss[1]
                        };
                        var contains = InfoHolder.ChartVariables.FirstOrDefault(x => x.Name == cv.Name);
                        if (contains == null)
                        {
                            InfoHolder.ChartVariables.Add(cv);
                            InfoHolder.OnChartVariablesChangeWrapper();
                        }
                        else
                        {
                            contains.Value = cv.Value;
                            contains.PasteToTheEnd(contains.GetFloat());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void OnOutcadVarsChanged(object sender, EventArgs args)
        {
            try
            {
                // outcad variables
                string outcadVarsString = outcadChannel.OutString;
                if (outcadVarsString != "null" && outcadVarsString != string.Empty)
                {
                    string[] vars = outcadVarsString.Split('&');
                    foreach (var v in vars)
                    {
                        string[] paramss = v.Split('#');
                        MainWindow.loggerPage.AppendTextToLog(paramss[0], string.Format("#{0}", paramss[1]));
                    }
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void OnRPIDataVarsChanged(object sender, EventArgs args)
        {
            try
            {
                // rpi data variables
                string rpiVarsString = rpiDataChannel.OutString;
                if (rpiVarsString != "null" && rpiVarsString != string.Empty)
                {
                    string[] paramss = rpiVarsString.Split('&');
                    if (paramss.Length == 8)
                    {
                        lastRPIDataUpdate = DateTime.Now;
                        try
                        {
                            InfoHolder.CurrentRPIData.Temperature = float.Parse(paramss[0], CultureInfo.InvariantCulture);
                            InfoHolder.CurrentRPIData.Memory = float.Parse(paramss[1], CultureInfo.InvariantCulture);
                            InfoHolder.CurrentRPIData.CPU = float.Parse(paramss[2], CultureInfo.InvariantCulture);
                            InfoHolder.CurrentRPIData.Power = float.Parse(paramss[3], CultureInfo.InvariantCulture);
                            InfoHolder.CurrentRPIData.SPITime = paramss[4];
                            InfoHolder.CurrentRPIData.SPIRxTime = paramss[5];
                            InfoHolder.CurrentRPIData.SPITxTime = paramss[6];
                            InfoHolder.CurrentRPIData.SPICallsPerTime = paramss[7];

                            OnRPIDataChange?.Invoke(null, EventArgs.Empty);
                        }
                        catch (FormatException ex)
                        {
                            InfoHolder.UserLogger.LogWrite(paramss[0]);
                            InfoHolder.UserLogger.LogWrite(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void OnCameraVarsChanged(object sender, EventArgs args)
        {
            try
            {
                // camera variables
                if (cameraChannel.OutBytes.Length > 0 && cameraChannel.OutString != "null")
                {
                    string[] paramss = cameraChannel.OutString.Split(';');
                    var cv = new CameraVariable()
                    {
                        Name = paramss[0],
                        Shape = paramss[1].Split(':')
                    };
                    var v = InfoHolder.CameraVariables.FirstOrDefault(x => x.Name == cv.Name);
                    if (v != null)
                    {
                        v.Value = cameraChannel.OutBytes;
                    }
                    else
                    {
                        cv.Value = cameraChannel.OutBytes;
                        InfoHolder.CameraVariables.Add(cv);
                        InfoHolder.OnCameraVariablesChangeWrapper();
                    }
                }
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private static void SetUpChannels()
        {
            inVariablesChannel = new ListenPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63253));
            outVariablesChannel = new TalkPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63258));
            chartsChannel = new ListenPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63255));
            outcadChannel = new ListenPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63257));
            rpiDataChannel = new ListenPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63256));
            cameraChannel = new ListenPort(new IPEndPoint(IPAddress.Parse(InfoHolder.CurrentSettings.IpAddress), 63254), true);
        }

        private static void LinkEvents()
        {
            inVariablesChannel.OnGotMessage += new EventHandler<EventArgs>(OnInVarsChanged);
            outVariablesChannel.OnNeedToSetMessage += new EventHandler<EventArgs>(OnOutVarsNeedToBeSet);
            chartsChannel.OnGotMessage += new EventHandler<EventArgs>(OnChartVarsChanged);
            outcadChannel.OnGotMessage += new EventHandler<EventArgs>(OnOutcadVarsChanged);
            rpiDataChannel.OnGotMessage += new EventHandler<EventArgs>(OnRPIDataVarsChanged);
            cameraChannel.OnGotMessage += new EventHandler<EventArgs>(OnCameraVarsChanged);
        }

        private static bool CheckAlive()
        {
            return inVariablesChannel.IsAlive() && outcadChannel.IsAlive() &&
                rpiDataChannel.IsAlive() && cameraChannel.IsAlive() &&
                chartsChannel.IsAlive() && outVariablesChannel.IsAlive();
        }

        private static void StartChannels()
        {
            inVariablesChannel.StartListening();
            outVariablesChannel.StartTalking();
            chartsChannel.StartListening();
            outcadChannel.StartListening();
            rpiDataChannel.StartListening();
            cameraChannel.StartListening();
        }

        private static void StopChannesls()
        {
            inVariablesChannel.StopListening();
            outVariablesChannel.StopTalking();
            chartsChannel.StopListening();
            outcadChannel.StopListening();
            rpiDataChannel.StopListening();
            cameraChannel.StopListening();
        }

        async public static Task StopHelper()
        {
            stopTask = true;
            await helperTask;
            try
            {
                StopChannesls();
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}
