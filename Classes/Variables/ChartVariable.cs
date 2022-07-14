using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shufflecad_4.Classes.Variables
{
    public class ChartVariable : ShuffleVariable
    {
        private const int DATA_SIZE = 1024;

        public float[] Data = new float[DATA_SIZE];

        public void ResetData()
        {
            Data = new float[DATA_SIZE];
        }

        public void PasteToTheEnd(float value)
        {
            Array.Copy(Data, 1, Data, 0, Data.Length - 1);
            Data[Data.Length - 1] = value;
        }
    }
}
