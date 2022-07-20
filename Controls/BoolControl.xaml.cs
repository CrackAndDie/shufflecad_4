using MaterialDesignThemes.Wpf;
using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
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
    public partial class BoolControl : UserControl, IRemoveable, IHaveVariable
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
                StateToggle.IsHitTestVisible = false;
                SetState(this.variable.GetBool());
                // подписываемся на изменения переменной
                this.variable.PropertyChanged += OnPropertyChanged;
            }
            else
            {
                // меняем картинки на кнопке
                StateToggle.Content = new PackIcon() { Kind = PackIconKind.Close };
                ToggleButtonAssist.SetOnContent(StateToggle, new PackIcon() { Kind = PackIconKind.Check });

                SetColor(false);
            }
        }

        public IFrontVariable GetVariable()
        {
            return this.variable;
        }

        // in bool control
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // если поменялось именно значение переменной
                if (args.PropertyName == "Value")
                {
                    this.SetState(this.variable.GetBool());
                }
            });
        }

        private void SetState(bool state)
        {
            if (state)
            {
                StateToggle.IsChecked = true;
                SetColor(true);
            }
            else
            {
                StateToggle.IsChecked = false;
                SetColor(false);
            }
        }

        // out bool control
        private void StateToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.variable.Direction == ShuffleVariable.OUT_DIR)
            {
                this.variable?.SetBool(true);
                SetColor(true);
            }
        }

        private void StateToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.variable.Direction == ShuffleVariable.OUT_DIR)
            {
                this.variable?.SetBool(false);
                SetColor(false);
            }
        }

        private void SetColor(bool state)
        {
            if (state)
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(trueColor);
            else
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(falseColor);
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
