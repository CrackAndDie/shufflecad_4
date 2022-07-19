using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for SliderControl.xaml
    /// </summary>
    public partial class SliderControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        private const float freqPres = 1000;

        public SliderControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;

            if (this.variable.Direction == ShuffleVariable.IN_DIR)
            {
                StateSlider.IsHitTestVisible = false;
                StateSlider.Value = this.variable.GetFloat();
                // подписываемся на изменения переменной
                this.variable.PropertyChanged += OnPropertyChanged;
            }

            ChangeFreq();

            DataContext = this.variable;
        }

        // in slider control
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // если поменялось именно значение переменной
                if (args.PropertyName == "Value")
                {
                    StateSlider.Value = this.variable.GetFloat();
                }
            });
        }


        // out slider control
        private void StateSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.variable.Direction == ShuffleVariable.OUT_DIR)
            {
                this.variable.SetFloat((float)StateSlider.Value);
            }
        }

        private void MinTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MinTB.Text.Length > 0)
            {
                bool parsed = float.TryParse(MinTB.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
                if (parsed)
                    StateSlider.Minimum = result;
            }
            else
            {
                StateSlider.Minimum = 0;
            }
            ChangeFreq();
        }

        private void MaxTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MaxTB.Text.Length > 0)
            {
                bool parsed = float.TryParse(MaxTB.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
                if (parsed)
                    StateSlider.Maximum = result;
            }
            else
            {
                StateSlider.Maximum = 0;
            }
            ChangeFreq();
        }

        private void ChangeFreq()
        {
            StateSlider.TickFrequency = Math.Abs(StateSlider.Maximum - StateSlider.Minimum) / freqPres;
        }

        Regex numEx = new Regex(@"^-?\d*\.?\d*$");

        private void TextTB_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text + e.Text;
            e.Handled = !numEx.IsMatch(text);
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}
