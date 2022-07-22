using MaterialDesignThemes.Wpf;
using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls.Interfaces;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for FloatControl.xaml
    /// </summary>
    public partial class FloatControl : UserControl, IRemoveable, IHaveVariable
    {
        private readonly ShuffleVariable variable;

        public event EventHandler OnRemove;

        public FloatControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;

            if (this.variable.Direction == ShuffleVariable.IN_DIR)
            {
                TextFieldAssist.SetHasClearButton(TextTB, false);
                TextTB.IsReadOnly = true;
                SetText(variable.GetString());
                // подписываемся на изменения переменной
                this.variable.PropertyChanged += OnPropertyChanged;
            }

            DataContext = this.variable;
        }

        public IFrontVariable GetVariable()
        {
            return this.variable;
        }

        // in float control
        private void SetText(string text)
        {
            TextTB.Text = text;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // если поменялось именно значение переменной
                if (args.PropertyName == "Value")
                {
                    SetText(variable.GetString());
                }
            });
        }

        // out float control
        Regex numEx = new Regex(@"^-?\d*\.?\d*$");
        private void TextTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text + e.Text;
            e.Handled = !numEx.IsMatch(text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.variable.Direction == ShuffleVariable.OUT_DIR)
            {
                if (TextTB.Text.Length > 0)
                {
                    this.variable.SetString(TextTB.Text);
                }
                else
                {
                    this.variable.SetString("0");
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            OnRemove?.Invoke(this, EventArgs.Empty);
        }
    }
}
