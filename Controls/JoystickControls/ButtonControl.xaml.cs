using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        public string ButtonShape
        {
            get { return (string)GetValue(ButtonShapeProperty); }
            set { SetValue(ButtonShapeProperty, value); }
        }

        public static readonly DependencyProperty ButtonShapeProperty =
            DependencyProperty.Register("ButtonShape", typeof(string), typeof(ButtonControl), new PropertyMetadata("", ButtonShapeChangedCallback));

        private static void ButtonShapeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctrl = dependencyObject as ButtonControl;
            if (ctrl != null)
            {
                if (dependencyPropertyChangedEventArgs.NewValue != null)
                {
                    if (dependencyPropertyChangedEventArgs.NewValue.ToString() == "Small")
                    {
                        ctrl.MainBorder.Width = 50;
                        ctrl.MainBorder.Height = 25;
                        ctrl.MainBorder.CornerRadius = new CornerRadius(5);
                    }
                    else
                    {
                        ctrl.MainBorder.Width = 50;
                        ctrl.MainBorder.Height = 55;
                        ctrl.MainBorder.CornerRadius = new CornerRadius(8, 8, 0, 0);
                    }
                }
            }
        }

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
