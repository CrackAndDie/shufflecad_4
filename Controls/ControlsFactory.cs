using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls.Interfaces;
using System.Windows;

namespace shufflecad_4.Controls
{
    public class ControlsFactory : IFactory
    {
        // singletone cringe
        private static ControlsFactory factory;

        public static ControlsFactory GetFactory()
        {
            if (factory == null)
            {
                factory = new ControlsFactory();
            }
            return factory;
        }

        public FrameworkElement Create(IFrontVariable variable)
        {
            switch (variable.Type)
            {
                // они имеют одинаковый контрол
                case ShuffleVariable.STRING_TYPE:
                case ShuffleVariable.BIG_STRING_TYPE:
                    {
                        return new StringControl(variable as ShuffleVariable);
                    }
                case ShuffleVariable.BOOL_TYPE:
                    {
                        return new BoolControl(variable as ShuffleVariable);
                    }
                case ShuffleVariable.FLOAT_TYPE:
                    {
                        return new FloatControl(variable as ShuffleVariable);
                    }
                case ShuffleVariable.CHART_TYPE:
                    {
                        break;
                    }
            }
            return null;
        }
    }
}
