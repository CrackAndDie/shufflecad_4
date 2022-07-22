using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;

namespace shufflecad_4.Classes
{
    public class SavingVariableClass
    {
        public IFrontVariable Variable { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
    }

    public class OpenSavingVariableClass
    {
        public DefaultVariable Variable { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
    }
}
