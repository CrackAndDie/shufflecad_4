using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for InBoolControl.xaml
    /// </summary>
    public partial class InBoolControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        private const string falseColor = "#cf5353";
        private const string trueColor = "#52a85b";

        public InBoolControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            SetState(this.variable.GetBool());

            // подписываемся на изменения переменной
            this.variable.PropertyChanged += OnPropertyChanged;

            DataContext = this.variable;
        }

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
                StateToggle.IsChecked = true;
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(trueColor);
            }
            else
            {
                StateToggle.IsChecked = false;
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(falseColor);
            }
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
