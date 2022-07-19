using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes;
using shufflecad_4.Helpers;
using System.Collections.Generic;
using System;
using shufflecad_4.Classes.Variables.Interfaces;
using System.Linq;

namespace shufflecad_4.Holders
{
    public class InfoHolder
    {
        public static int CurrentSelectedCamera = 0;
        public static bool CameraVariablesAllGot = false;

        public static SettingsClass CurrentSettings { get; set; }

        public static RPIDataClass CurrentRPIData = new RPIDataClass();

        public static UserLogHelper UserLogger = new UserLogHelper("Begin");

        public static List<IFrontVariable> AllVariables = new List<IFrontVariable>();

        public static List<CameraVariable> CameraVariables = new List<CameraVariable>();

        public static List<IFrontVariable> VariablesOnFrontPanel = new List<IFrontVariable>();

        public static event EventHandler<EventArgs> OnAllVariablesChange;
        public static event EventHandler<EventArgs> OnCameraVariablesChange;

        static InfoHolder()
        {
            ConnectionHelper.OnDisconnect += new EventHandler<EventArgs>(ClearAll);            
        }

        public static void OnAllVariablesChangeWrapper()
        {
            if (OnAllVariablesChange != null)
            {
                OnAllVariablesChange(null, EventArgs.Empty);
            }
        }

        public static void OnCameraVariablesChangeWrapper()
        {
            if (OnCameraVariablesChange != null)
            {
                OnCameraVariablesChange(null, EventArgs.Empty);
            }
        }

        public static void ClearAll(object sender, EventArgs args)
        {
            CurrentRPIData.ClearAll();
            ClearAllExceptThatOnFrontPanel();
            CameraVariables.Clear();

            OnAllVariablesChange(null, EventArgs.Empty);
            OnCameraVariablesChange(null, EventArgs.Empty);

            CurrentRPIData.OnRPIDataChange(null, EventArgs.Empty);
        }

        private static void ClearAllExceptThatOnFrontPanel()
        {
            List<IFrontVariable> exceptFront = AllVariables.Except(VariablesOnFrontPanel).ToList();
            foreach (var v in exceptFront)
            {
                AllVariables.Remove(v);
            }
        }
    }
}
