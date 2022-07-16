using shufflecad_4.Classes.Variables.Interfaces;
using System.Windows;

namespace shufflecad_4.Controls.Interfaces
{
    public interface IFactory
    {
        FrameworkElement Create(IFrontVariable variable);
    }
}
