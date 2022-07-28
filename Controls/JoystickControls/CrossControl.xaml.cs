using shufflecad_4.Helpers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for CrossControl.xaml
    /// </summary>
    public partial class CrossControl : UserControl
    {
        private static readonly float[] inArr = { -1, 1 };
        private static readonly float[] outArr = { 0, 1 };

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
                angle -= 90;
                double rad = Math.PI * angle / 180f;
                double x = FuncadHelper.Transfunc((float)Math.Cos(rad), inArr.ToList(), outArr.ToList());
                double y = FuncadHelper.Transfunc((float)Math.Sin(rad), inArr.ToList(), outArr.ToList());
                CrossGradient.GradientOrigin = new Point(x, y);
            }
        }
    }
}
