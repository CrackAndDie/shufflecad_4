using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes;
using shufflecad_4.Helpers;
using System.Collections.Generic;
using System;

namespace shufflecad_4.Holders
{
    public class InfoHolder
    {
        public static int CurrentSelectedCamera = 0;
        public static bool CameraVariablesAllGot = false;

        public static SettingsClass CurrentSettings { get; set; }

        public static RPIDataClass CurrentRPIData = new RPIDataClass();

        public static UserLogHelper UserLogger = new UserLogHelper("Begin");

        public static List<ShuffleVariable> OutVariables = new List<ShuffleVariable>();

        public static List<ShuffleVariable> InVariables = new List<ShuffleVariable>();

        public static List<ChartVariable> ChartVariables = new List<ChartVariable>();

        public static List<CameraVariable> CameraVariables = new List<CameraVariable>();

        public static event EventHandler<EventArgs> OnOutVariablesChange;
        public static event EventHandler<EventArgs> OnInVariablesChange;
        public static event EventHandler<EventArgs> OnChartVariablesChange;
        public static event EventHandler<EventArgs> OnCameraVariablesChange;

        static InfoHolder()
        {
            ConnectionHelper.OnDisconnect += new EventHandler<EventArgs>(ClearAll);
        }

        public static void OnOutVariablesChangeWrapper()
        {
            if (OnOutVariablesChange != null)
            {
                OnOutVariablesChange(null, EventArgs.Empty);
            }
        }

        public static void OnInVariablesChangeWrapper()
        {
            if (OnInVariablesChange != null)
            {
                OnInVariablesChange(null, EventArgs.Empty);
            }
        }

        public static void OnChartVariablesChangeWrapper()
        {
            if (OnChartVariablesChange != null)
            {
                OnChartVariablesChange(null, EventArgs.Empty);
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
            OutVariables.Clear();
            InVariables.Clear();
            ChartVariables.Clear();
            CameraVariables.Clear();

            OnOutVariablesChange(null, EventArgs.Empty);
            OnInVariablesChange(null, EventArgs.Empty);
            OnChartVariablesChange(null, EventArgs.Empty);
            OnCameraVariablesChange(null, EventArgs.Empty);

            CurrentRPIData.OnRPIDataChange(null, EventArgs.Empty);
        }
    }
}
