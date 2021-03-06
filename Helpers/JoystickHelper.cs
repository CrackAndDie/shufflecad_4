using SharpDX;
using SharpDX.DirectInput;
using shufflecad_4.Classes;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shufflecad_4.Helpers
{
    internal class JoystickHelper
    {
        public static List<JoystickClass> JoystickValues = new List<JoystickClass>();
        public static string CurrentlyChangedOffset;
        public static event EventHandler OnJoyValueChange;

        private static Task taskJoystick;
        private static bool stopJoystick;

        private static Joystick joystick;

        public static void StartJoystick()
        {
            stopJoystick = false;
            taskJoystick = Task.Run(() =>
            {
                while (!stopJoystick)
                {
                    try
                    {
                        if (joystick == null)
                        {
                            var directInput = new DirectInput();

                            var joystickGuid = Guid.Empty;

                            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                                        DeviceEnumerationFlags.AllDevices))
                                joystickGuid = deviceInstance.InstanceGuid;

                            if (joystickGuid == Guid.Empty)
                                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                                        DeviceEnumerationFlags.AllDevices))
                                    joystickGuid = deviceInstance.InstanceGuid;

                            if (joystickGuid == Guid.Empty)
                            {
                                Thread.Sleep(500);
                                continue;
                            }

                            joystick = new Joystick(directInput, joystickGuid);
                            joystick.Properties.BufferSize = 128;
                            joystick.Acquire();

                            InfoHolder.CurrentRPIData.JIC = true;
                        }
                        else
                        {
                            try
                            {
                                joystick.Poll();
                                var datas = joystick.GetBufferedData();
                                foreach (var state in datas)
                                {
                                    var element = JoystickValues.FirstOrDefault(x => x.Name == state.Offset.ToString());
                                    if (element != null)
                                    {
                                        element.Value = state.Value;
                                    }
                                    else
                                    {
                                        JoystickValues.Add(new JoystickClass()
                                        {
                                            Name = state.Offset.ToString(), 
                                            Value = state.Value 
                                        });
                                    }

                                    CurrentlyChangedOffset = state.Offset.ToString();
                                    OnJoyValueChange?.Invoke(null, EventArgs.Empty);
                                }
                            }
                            catch (SharpDXException)
                            {
                                joystick = null;
                                InfoHolder.CurrentRPIData.JIC = false;
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

        async public static Task StopJoystick()
        {
            stopJoystick = true;
            await taskJoystick;
            joystick?.Unacquire();
        }
    }
}
