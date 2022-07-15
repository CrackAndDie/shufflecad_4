using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shufflecad_4.Classes.Variables.Interfaces
{
    public interface IFrontVariable : IVariable
    {
        string Type { get; set; }
        string Value { get; set; }
        string Direction { get; set; }

        bool GetBool();
        float GetFloat();
        string GetString();
        void SetBool(bool value);
        void SetFloat(float value);
        void SetString(string value);
    }
}
