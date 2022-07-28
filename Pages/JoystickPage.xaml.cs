using shufflecad_4.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for JoystickPage.xaml
    /// </summary>
    public partial class JoystickPage : Page
    {
        public JoystickPage()
        {
            InitializeComponent();

            JoystickHelper.OnJoyValueChange += OnJoyValuesChange;
        }

        private void OnJoyValuesChange(object sender, EventArgs args)
        {
            int changedValue = JoystickHelper.JoystickValues[JoystickHelper.CurrentlyChangedOffset];
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (JoystickHelper.CurrentlyChangedOffset)
                {
                    case "Buttons0":
                        {
                            AButtonControl.SetValue(changedValue);
                            break;
                        }
                    case "Buttons1":
                        {
                            BButtonControl.SetValue(changedValue);
                            break;
                        }
                    case "Buttons2":
                        {
                            XButtonControl.SetValue(changedValue);
                            break;
                        }
                    case "Buttons3":
                        {
                            YButtonControl.SetValue(changedValue);
                            break;
                        }
                    case "X":
                        {
                            LeftStickControl.SetValueX(changedValue);
                            break;
                        }
                    case "Y":
                        {
                            LeftStickControl.SetValueY(changedValue);
                            break;
                        }
                    case "RotationX":
                        {
                            RightStickControl.SetValueX(changedValue);
                            break;
                        }
                    case "RotationY":
                        {
                            RightStickControl.SetValueY(changedValue);
                            break;
                        }
                    case "PointOfViewControllers0":
                        {
                            ThisCrossControl.SetValue(changedValue);
                            break;
                        }
                    case "Buttons8":
                        {
                            LeftStickControl.SetClickValue(changedValue);
                            break;
                        }
                    case "Buttons9":
                        {
                            RightStickControl.SetClickValue(changedValue);
                            break;
                        }
                    case "Z":
                        {
                            if (changedValue == 128)
                            {
                                RightUpButton.SetValue(128);
                            }
                            else if (changedValue == 65408)
                            {
                                LeftUpButton.SetValue(128);
                            }
                            else
                            {
                                RightUpButton.SetValue(0);
                                LeftUpButton.SetValue(0);
                            }
                            break;
                        }
                    case "Buttons4":
                        {
                            LeftBottomButton.SetValue(changedValue);
                            break;
                        }
                    case "Buttons5":
                        {
                            RightBottomButton.SetValue(changedValue);
                            break;
                        }
                    case "Buttons6":
                        {
                            LeftCenterButton.SetValue(changedValue);
                            break;
                        }
                    case "Buttons7":
                        {
                            RightCenterButton.SetValue(changedValue);
                            break;
                        }
                }
            });
           
        }
    }
}
