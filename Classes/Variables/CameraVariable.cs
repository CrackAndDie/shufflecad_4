using shufflecad_4.Classes.Variables.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shufflecad_4.Classes.Variables
{
    public class CameraVariable : INotifyPropertyChanged, IVariable
    {
        private byte[] _value;
        private string _name;
        private string[] _shape;
        public byte[] Value { get { return _value; } set { _value = value; NotifyPropertyChanged("Value"); } }
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public string[] Shape { get { return _shape; } set { _shape = value; NotifyPropertyChanged("Shape"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
