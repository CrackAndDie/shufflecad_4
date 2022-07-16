using shufflecad_4.Classes.Variables;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for OutStringControl.xaml
    /// </summary>
    public partial class OutStringControl : UserControl
    {
        private readonly ShuffleVariable variable;

        public OutStringControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;

            DataContext = this.variable;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.variable.SetString(TextTB.Text);
        }
    }
}
