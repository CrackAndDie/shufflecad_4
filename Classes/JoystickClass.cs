using System;
using System.ComponentModel;

namespace shufflecad_4.Classes
{
    internal class JoystickClass : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        private int _value;
        public int Value { get { return _value; } set { _value = value; NotifyPropertyChanged("Value"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
