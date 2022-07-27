using System;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for CrossControl.xaml
    /// </summary>
    public partial class CrossControl : UserControl
    {
        public CrossControl()
        {
            InitializeComponent();
        }

        public void SetValue(int angle)
        {
            if (angle == -1)
            {
                CrossGradient.GradientOrigin = new Point(0.5, 0.5);
            }
            else
            {
                angle /= 100;
                double rad = Math.PI * angle / 180f;
                double x = Math.Cos(rad);
                double y = Math.Sin(rad);
                CrossGradient.GradientOrigin = new Point(x, y);
            }
        }
    }
}
