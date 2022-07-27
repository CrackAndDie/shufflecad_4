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
        private const int MIDDLE = 32767;

        private static readonly float[] inArr = { 0, 65535 };
        private static readonly float[] outArr = { -25, 25 };

        private int currentX = MIDDLE;
        private int currentY = MIDDLE;

        public StickControl()
        {
            InitializeComponent();
        }

        public void SetValueX(int value)
        {
            currentX = value;
            Update();
        }

        public void SetValueY(int value)
        {
            currentY = value;
            Update();
        }

        public void Update()
        {
            int xMargin = (int)FuncadHelper.Transfunc(currentX, inArr.ToList(), outArr.ToList());
            int yMargin = (int)FuncadHelper.Transfunc(currentY, inArr.ToList(), outArr.ToList());
            Stick.Margin = new System.Windows.Thickness(xMargin, yMargin, 0, 0);
        }
    }
}
