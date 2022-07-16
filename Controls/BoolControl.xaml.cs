using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for BoolControl.xaml
    /// </summary>
    public partial class BoolControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        private const string falseColor = "#cf5353";
        private const string trueColor = "#52a85b";

        public BoolControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            DataContext = this.variable;

            if (this.variable.Direction == ShuffleVariable.IN_DIR)
            {
                SetState(this.variable.GetBool());
                // подписываемся на изменения переменной
                this.variable.PropertyChanged += OnPropertyChanged;
            }
            else
            {
                InputToggle.Visibility = Visibility.Collapsed;
                OutputToggle.Visibility = Visibility.Visible;

                SetColor(false);
            }
        }

        // in bool control
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // если поменялось именно значение переменной
            if (args.PropertyName == "Value")
            {
                this.SetState(this.variable.GetBool());
            }
        }

        private void SetState(bool state)
        {
            if (state)
            {
                InputToggle.IsChecked = true;
                InputToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(trueColor);
            }
            else
            {
                InputToggle.IsChecked = false;
                InputToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(falseColor);
            }
        }

        // out bool control
        private void StateToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.variable?.SetBool(true);
            SetColor(true);
        }

        private void StateToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.variable?.SetBool(false);
            SetColor(false);
        }

        private void SetColor(bool state)
        {
            if (state)
                OutputToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(trueColor);
            else
                OutputToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(falseColor);
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
