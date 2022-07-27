using SharpDX;
using SharpDX.DirectInput;
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
        public static IDictionary<string, int> JoystickValues = new Dictionary<string, int>();

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
                                    if (JoystickValues.ContainsKey(state.Offset.ToString()))
                                    {
                                        JoystickValues[state.Offset.ToString()] = state.Value;
                                    }
                                    else
                                    {
                                        JoystickValues.Add(state.Offset.ToString(), state.Value);
                                    }
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
