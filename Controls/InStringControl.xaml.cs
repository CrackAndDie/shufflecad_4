using shufflecad_4.Classes.Variables;
using System.ComponentModel;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for InStringControl.xaml
    /// </summary>
    public partial class InStringControl : UserControl
    {
        private readonly ShuffleVariable variable;

        public InStringControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            // подписываемся на изменения переменной
            this.variable.PropertyChanged += OnPropertyChanged;

            DataContext = this.variable;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // если поменялось именно значение переменной
            if (args.PropertyName == "Value")
            {
                TextTB.Text = variable.GetString();
            }
        }
    }
}
