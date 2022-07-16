﻿using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for InBigStringControl.xaml
    /// </summary>
    public partial class InBigStringControl : UserControl, IRemoveable
    {
        private readonly ShuffleVariable variable;

        public InBigStringControl(ShuffleVariable variable)
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

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
