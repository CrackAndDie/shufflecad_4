﻿using shufflecad_4.Classes.Variables;
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
    /// Interaction logic for OutBoolControl.xaml
    /// </summary>
    public partial class OutBoolControl : UserControl
    {
        private readonly ShuffleVariable variable;

        private const string falseColor = "#cf5353";
        private const string trueColor = "#52a85b";

        public OutBoolControl(ShuffleVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            SetColor(false);

            DataContext = this.variable;
        }

        private void StateToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.variable?.SetBool(true);
            SetColor(true);
        }

        private void StateToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.variable?.SetBool(false);
            SetColor(false);
        }

        private void SetColor(bool state)
        {
            if (state)
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(trueColor);
            else
                StateToggle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(falseColor);
        }
    }
}