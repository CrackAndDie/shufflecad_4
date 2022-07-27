using shufflecad_4.Helpers;
using System.Linq;
using System.Windows.Controls;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for StickControl.xaml
    /// </summary>
    public partial class StickControl : UserControl
    {
        private static readonly float[] inArr = { 0, 65535 };
        private static readonly float[] outArr = { -10, 10 };

        public StickControl()
        {
            InitializeComponent();
        }

        public void SetValue(int x, float y)
        {
            int xMargin = (int)FuncadHelper.Transfunc(x, inArr.ToList(), outArr.ToList());
            int yMargin = (int)FuncadHelper.Transfunc(y, inArr.ToList(), outArr.ToList());
            Stick.Margin = new System.Windows.Thickness(xMargin, yMargin, 0, 0);
        }
    }
}
