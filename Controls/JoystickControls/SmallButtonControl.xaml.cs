using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for SmallButtonControl.xaml
    /// </summary>
    public partial class SmallButtonControl : UserControl
    {
        Color defaultUpperColor;
        Color defaultLowerColor;

        public SmallButtonControl()
        {
            InitializeComponent();

            defaultUpperColor = UpperStop.Color;
            defaultLowerColor = LowerStop.Color;
        }

        public void SetValue(int value)
        {
            if (value == 128)
            {
                UpperStop.Color = (Color)ColorConverter.ConvertFromString("#111111");
                LowerStop.Color = (Color)ColorConverter.ConvertFromString("#2A2A2A");
            }
            else
            {
                UpperStop.Color = defaultUpperColor;
                LowerStop.Color = defaultLowerColor;
            }
        }
    }
}
