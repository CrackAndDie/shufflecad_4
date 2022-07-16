using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for OutBigStringControl.xaml
    /// </summary>
    public partial class OutBigStringControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        public OutBigStringControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;

            DataContext = this.variable;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.variable.SetString(TextTB.Text);
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
