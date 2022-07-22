using shufflecad_4.Classes.Variables.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;

namespace shufflecad_4.Classes.Variables
{
    public class ShuffleVariable : INotifyPropertyChanged, IFrontVariable
    {
        public const string FLOAT_TYPE = "float";
        public const string STRING_TYPE = "string";
        public const string BIG_STRING_TYPE = "bigstring";
        public const string BOOL_TYPE = "bool";
        public const string CHART_TYPE = "chart";
        public const string SLIDER_TYPE = "slider";

        public const string IN_DIR = "in";
        public const string OUT_DIR = "out";

        private string _value;
        private string _type;
        private string _name;
        private string _direction;

        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public string Value { get { return _value; } set { _value = value; NotifyPropertyChanged("Value"); } }
        public string Type { get { return _type; } set { _type = value; NotifyPropertyChanged("Type"); } }
        public string Direction { get { return _direction; } set { _direction = value; NotifyPropertyChanged("Direction"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public ShuffleVariable()
        {

        }

        public ShuffleVariable(DefaultVariable dv)
        {
            this.Name = dv.Name;
            this.Type = dv.Type;
            this.Direction = dv.Direction;
            this.Value = dv.Value;
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public float GetFloat()
        {
            bool parsed = float.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
            if (parsed)
                return result;
            return 0;
        }

        public bool GetBool()
        {
            return Value == "1";
        }

        public string GetString()
        {
            return Value;
        }

        public void SetFloat(float value)
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetBool(bool value)
        {
            Value = value ? "1" : "0";
        }

        public void SetString(string value)
        {
            Value = value;
        }
    }
}
