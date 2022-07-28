using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        Color defaultUpperColor;
        Color defaultLowerColor;

        public ButtonControl()
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
