using shufflecad_4.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace shufflecad_4.Classes
{
    public class RPIDataClass
    {
        // добавлены средние значения, чтоб был нормальный желтый
        private static List<float> trans514In = new List<float>() { 10, 11.5f, 13 };
        private static List<float> trans100In = new List<float>() { 0, 50, 100 };
        private static List<float> trans3080In = new List<float>() { 30, 55, 80 };
        private static List<float> transMaxToMin255Out = new List<float>() { 210, 180, 0 };
        private static List<float> transMinToMax255Out = new List<float>() { 0, 180, 210 };

        private static SolidColorBrush greenColor = new SolidColorBrush(Color.FromArgb(255, 40, 210, 50));
        private static SolidColorBrush redColor = new SolidColorBrush(Color.FromArgb(255, 230, 30, 50));

        // main window
        private float power;
        public float Power 
        {
            get
            {
                return power;
            }
            set 
            { 
                this.power = value;
                PowerChanged();
            } 
        }
        private float temperature;
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                this.temperature = value;
                TemperatureChanged();
            }
        }
        private float memory;
        public float Memory
        {
            get
            {
                return memory;
            }
            set
            {
                this.memory = value;
                MemoryChanged();
            }
        }
        private float cpu;
        public float CPU
        {
            get
            {
                return cpu;
            }
            set
            {
                this.cpu = value;
                CPUChanged();
            }
        }
        private bool ric;
        public bool RIC
        {
            get
            {
                return ric;
            }
            set
            {
                this.ric = value;
                RICChanged();
            }
        }
        private bool pir;
        public bool PIR
        {
            get
            {
                return pir;
            }
            set
            {
                this.pir = value;
                PIRChanged();
            }
        }
        private bool pic;
        public bool PIC
        {
            get
            {
                return pic;
            }
            set
            {
                this.pic = value;
                PICChanged();
            }
        }

        private void PowerChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().BatteryVoltage.Text = this.power.ToString("0.0");
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().PowerBorder.Background = new SolidColorBrush(Color.FromArgb(255,
                                                    (byte)(int)FuncadHelper.Transfunc(this.power, trans514In, transMaxToMin255Out),
                                                    (byte)(int)FuncadHelper.Transfunc(this.power, trans514In, transMinToMax255Out), 0));
            });
        }

        private void TemperatureChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Temperature.Text = this.temperature.ToString("0.0");
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().TempBorder.Background = new SolidColorBrush(Color.FromArgb(255,
                                                    (byte)(int)FuncadHelper.Transfunc(this.temperature, trans3080In, transMinToMax255Out),
                                                    (byte)(int)FuncadHelper.Transfunc(this.temperature, trans3080In, transMaxToMin255Out), 0));
            });
        }

        private void MemoryChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MemoryUsed.Text = this.memory.ToString("0.0");
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MemoryBorder.Background = new SolidColorBrush(Color.FromArgb(255,
                                                    (byte)(int)FuncadHelper.Transfunc(this.memory, trans100In, transMinToMax255Out),
                                                    (byte)(int)FuncadHelper.Transfunc(this.memory, trans100In, transMaxToMin255Out), 0));
            });
        }

        private void CPUChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().CPU1.Text = this.cpu.ToString("0.0");
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().CPU1Border.Background = new SolidColorBrush(Color.FromArgb(255,
                                                    (byte)(int)FuncadHelper.Transfunc(this.cpu, trans100In, transMinToMax255Out),
                                                    (byte)(int)FuncadHelper.Transfunc(this.cpu, trans100In, transMaxToMin255Out), 0));
            });
        }

        private void RICChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().RobotConnectedBorder.Background = ric ? greenColor : redColor;
            });
        }
        
        private void PIRChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().RunningBorder.Background = pir ? greenColor : redColor;
            });
        }

        private void PICChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().ConnectedBorder.Background = pic ? greenColor : redColor;
            });
        }

        // dev page
        public string VarsOutTime { get; set; }
        public string VarsInTime { get; set; }
        public string ChartsTime { get; set; }
        public string CameraTime { get; set; }
        public string OutcadTime { get; set; }
        public string RPIDataTime { get; set; }
        public string SPITime { get; set; }
        public string SPIRxTime { get; set; }
        public string SPITxTime { get; set; }
        public string SPICallsPerTime { get; set; }

        public void OnRPIDataChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindow.devPage.VarsInTimeTB.Text = VarsInTime;
                MainWindow.devPage.VarsOutTimeTB.Text = VarsOutTime;
                MainWindow.devPage.ChartsTimeTB.Text = ChartsTime;
                MainWindow.devPage.CameraTimeTB.Text = CameraTime;
                MainWindow.devPage.OutcadTimeTB.Text = OutcadTime;
                MainWindow.devPage.RPIDataTimeTB.Text = RPIDataTime;
                MainWindow.devPage.SPITimeTB.Text = SPITime;
                MainWindow.devPage.RxSpiPrepTB.Text = SPIRxTime;
                MainWindow.devPage.TxSpiPrepTB.Text = SPITxTime;
                MainWindow.devPage.SpiCountTB.Text = SPICallsPerTime;
            });
        }

        public void ClearAll()
        {
            Power = 0;
            Temperature = 0;
            Memory = 0;
            CPU = 0;

            VarsOutTime = "0";
            VarsInTime = "0";
            ChartsTime = "0";
            CameraTime = "0";
            OutcadTime = "0";
            RPIDataTime = "0";
            SPITime = "0";
            SPIRxTime = "0";
            SPITxTime = "0";
            SPICallsPerTime = "0";
        }
    }
}
