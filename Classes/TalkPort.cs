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
    public class TalkPort
    {
        public event EventHandler<EventArgs> OnNeedToSetMessage;

        public string OutString = "";
        private bool stopThread = false;
        private IPEndPoint ipPoint;

        private Socket sct;
        public Thread thread;

        private readonly int sleepTime;

        internal TalkPort(IPEndPoint ipPoint, int sleepTime)
        {
            this.ipPoint = ipPoint;
            this.sleepTime = sleepTime;
        }

        private void SetUpSocket()
        {
            sct = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sct.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void StartTalking()
        {
            OutString = "";
            this.stopThread = false;
            SetUpSocket();
            thread = new Thread(() => { Talking(); });
            thread.Start();
        }

        private void Talking()
        {
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
                    if (OnNeedToSetMessage != null)
                    {
                        OnNeedToSetMessage(this, EventArgs.Empty);
                    }

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
                        }
                        catch (SocketException exx)
                        {
                            // тут, если вызвалась остановка общения
                            InfoHolder.UserLogger.LogWrite(exx.ToString());
                            break;
                        }
                    }

                    FuncadHelper.SetOutputBytes(Encoding.ASCII.GetBytes(OutString), sct);
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

        public void StopTalking()
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
            ResetOut();
        }

        public void ResetOut()
        {
            OutString = "";
        }
    }
}
