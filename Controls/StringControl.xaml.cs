using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for StringControl.xaml
    /// </summary>
    public partial class StringControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        public StringControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;

            if (this.variable.Type == ShuffleVariable.BIG_STRING_TYPE)
            {
                TextTB.Width = 160;
                TextTB.MaxLength = 200;
            }

            if (this.variable.Direction == ShuffleVariable.IN_DIR)
            {
                TextTB.IsReadOnly = true;
                SetText(variable.GetString());
                // подписываемся на изменения переменной
                this.variable.PropertyChanged += OnPropertyChanged;
            }

            DataContext = this.variable;
        }

        // in string control
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

        // out string control
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.variable.Direction == ShuffleVariable.OUT_DIR)
            {
                this.variable.SetString(TextTB.Text);
            }
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
