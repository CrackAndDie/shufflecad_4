using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls.JoystickControls
{
    /// <summary>
    /// Interaction logic for LetterButtonControl.xaml
    /// </summary>
    public partial class LetterButtonControl : UserControl
    {
        public string ButtonLetter
        {
            get { return (string)GetValue(ButtonLetterProperty); }
            set { SetValue(ButtonLetterProperty, value); }
        }

        public static readonly DependencyProperty ButtonLetterProperty =
            DependencyProperty.Register("ButtonLetter", typeof(string), typeof(LetterButtonControl), new PropertyMetadata("", ButtonLetterChangedCallback));

        private static void ButtonLetterChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctrl = dependencyObject as LetterButtonControl;
            if (ctrl != null)
            {
                if (dependencyPropertyChangedEventArgs.NewValue != null)
                    ctrl.ButtonNameTB.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
            }
        }

        public string ButtonColor
        {
            get { return (string)GetValue(ButtonColorProperty); }
            set { SetValue(ButtonColorProperty, value); }
        }

        public static readonly DependencyProperty ButtonColorProperty =
            DependencyProperty.Register("ButtonColor", typeof(string), typeof(LetterButtonControl), new PropertyMetadata("", ButtonColorChangedCallback));

        private static void ButtonColorChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctrl = dependencyObject as LetterButtonControl;
            if (ctrl != null)
            {
                if (dependencyPropertyChangedEventArgs.NewValue != null)
                    ctrl.ButtonNameTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(dependencyPropertyChangedEventArgs.NewValue.ToString()));
            }
        }

        public LetterButtonControl()
        {
            InitializeComponent();
        }

        public void SetValue(int value)
        {
            BorderGradient.GradientOrigin = value == 128 ? new Point(0.3, 0.3) : new Point(0.5, 0.5);
            ButtonNameTB.Opacity = value == 128 ? 0.3 : 1;
        }
    }
}
