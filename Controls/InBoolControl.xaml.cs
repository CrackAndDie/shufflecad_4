using shufflecad_4.Classes.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for InBoolControl.xaml
    /// </summary>
    public partial class InBoolControl : UserControl
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
    }
}
