using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shufflecad_4.Classes
{
    public class ListenPort
    {
        public event EventHandler<EventArgs> OnGotMessage;

        public string SendString = "-1";
        public string OutString = string.Empty;
        public byte[] OutBytes = new byte[0];
        private bool stopThread = false;
        private IPEndPoint ipPoint;

        private Socket sct;
        public Thread thread;
        private bool isCamera;

        private readonly int sleepTime;

        internal ListenPort(IPEndPoint ipPoint, int sleepTime, bool isCamera = false)
        {
            this.ipPoint = ipPoint;
            this.isCamera = isCamera;
            this.sleepTime = sleepTime;
        }

        private void SetUpSocket()
        {
            sct = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sct.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void ResetString()
        {
            OutString = string.Empty;
            OutBytes = new byte[0];
            SendString = "-1";
        }

        public void StartListening()
        {
            this.stopThread = false;
            SetUpSocket();
            thread = new Thread(() => { Listening(); });
            thread.Start();
        }

        private void Listening()
        {
            // Debug.Log("ListenPort Started on port: " + this.ipPoint.Port.ToString());
            try
            {
                sct.Connect(ipPoint);
            }
            catch (SocketException exx)
            {
                // тут появляется, если сразу закрывать общение
                InfoHolder.UserLogger.LogWrite(exx.ToString());
                return;
            }
            while (!this.stopThread)
            {
                try
                {
                    string outString = Encoding.ASCII.GetString(FuncadHelper.GetInputBytes(sct));

                    // тут, если пользователь отключился, то в текущем задании будет пустая строка
                    if (outString == string.Empty)
                    {
                        sct.Shutdown(SocketShutdown.Both);
                        sct.Close();
                        try
                        {
                            SetUpSocket();
                            sct.Connect(ipPoint);
                            // continue;
                        }
                        catch (SocketException exx)
                        {
                            // тут, если вызвалась остановка общения
                            InfoHolder.UserLogger.LogWrite(exx.ToString());
                            break;
                        }
                    }

                    if (this.isCamera)
                    {
                        FuncadHelper.SetOutputBytes(new byte[4], sct);

                        byte[] data = FuncadHelper.GetInputBytes(sct);
                        OutBytes = data;
                    }

                    OutString = outString;

                    OnGotMessage?.Invoke(this, EventArgs.Empty);

                    FuncadHelper.SetOutputBytes(Encoding.ASCII.GetBytes(SendString), sct);
                    Thread.Sleep(this.sleepTime);
                }
                catch (Exception ex)
                {
                    // тут, если какая-то ошибка, но скорее всего из-за остановки общения
                    InfoHolder.UserLogger.LogWrite(ex.ToString());
                    try
                    {
                        sct.Shutdown(SocketShutdown.Both);
                        sct.Close();
                    }
                    catch (Exception e)
                    {
                        InfoHolder.UserLogger.LogWrite(e.ToString());
                        try
                        {
                            SetUpSocket();
                            sct.Connect(ipPoint);
                        }
                        catch (Exception exx)
                        {
                            // тут, если была остановка общения
                            InfoHolder.UserLogger.LogWrite(exx.ToString());
                            break;
                        }
                    }
                }
            }
        }

        public bool IsAlive()
        {
            if (thread != null)
                return thread.IsAlive;
            return false;
        }

        public void StopListening()
        {
            this.stopThread = true;
            try
            {
                sct.Shutdown(SocketShutdown.Both);
                sct.Close();
            }
            catch (Exception e)
            {
                InfoHolder.UserLogger.LogWrite(e.ToString());
                try
                {
                    sct.Close();
                }
                catch (Exception e2)
                {
                    InfoHolder.UserLogger.LogWrite(e2.ToString());
                }
            }
            ResetString();
        }
    }
}
