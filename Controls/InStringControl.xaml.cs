using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for InStringControl.xaml
    /// </summary>
    public partial class InStringControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        public InStringControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            SetText(variable.GetString());

            // подписываемся на изменения переменной
            this.variable.PropertyChanged += OnPropertyChanged;

            DataContext = this.variable;
        }

        private void SetText(string text)
        {
            TextTB.Text = text;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // если поменялось именно значение переменной
            if (args.PropertyName == "Value")
            {
                SetText(variable.GetString());
            }
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
