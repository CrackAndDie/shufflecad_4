using shufflecad_4.Classes.Variables;
using System;
using System.Collections.Generic;
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
