using System;

namespace shufflecad_4.Classes.Variables
{
    public class ChartVariable : ShuffleVariable
    {
        private const int DATA_SIZE = 256;

        public double[] Data = new double[DATA_SIZE];

        public event EventHandler<EventArgs> DataChanged;

        public ChartVariable() : base() { }
        public ChartVariable(DefaultVariable dv) : base(dv) { }

        public void ResetData()
        {
            Data = new double[DATA_SIZE];
        }

        public void PasteToTheEnd(float value)
        {
            Array.Copy(Data, 1, Data, 0, Data.Length - 1);
            Data[Data.Length - 1] = value;

            DataChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
