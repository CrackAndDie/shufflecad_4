using shufflecad_4.Classes;
using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using shufflecad_4.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<LanguageClass> languageClasses;

        public static MainWindow ThisMainWindow;

        public static ConnectionPage connectionPage;
        public static RunPage runPage;
        public static FrontPanelPage frontPanelPage;
        public static LoggerPage loggerPage;
        public static CameraPage cameraPage;
        public static SettingsPage settingsPage;
        public static DevPage devPage;
        public static JoystickPage joyPage;

        public MainWindow()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;

            ThisMainWindow = this;

            SetUpVisual();

            MainFrame.Content = connectionPage;
        }

        private void SetUpVisual()
        {
            // тут нужно как-то сосать
            languageClasses = new List<LanguageClass>()
            {
                new LanguageClass()
                {
                    LangShortName = "EN"
                },
            };

            ThisHeader.InitLangCB(languageClasses);

            connectionPage = new ConnectionPage();
            runPage = new RunPage();
            frontPanelPage = new FrontPanelPage();
            loggerPage = new LoggerPage();
            cameraPage = new CameraPage();
            devPage = new DevPage();
            joyPage = new JoystickPage();

            settingsPage = new SettingsPage();
            settingsPage.LoadAndSet();

            CommandsHelper.StartChecks();
            ConnectionHelper.StartHelper();
            JoystickHelper.StartJoystick();

            ConnectionHelper.OnRPIDataChange += new EventHandler<EventArgs>(InfoHolder.CurrentRPIData.OnRPIDataChange);

            loggerPage.AppendTextToLog("Shufflecad inited", "#00AEAE");

            ThisHeader.InitDone();
        }

        public const string STATE_DEFAULT_COLOR = "#F0F8FF";
        public const string STATE_WARNING_COLOR = "#DEAA1D";
        public const string STATE_ERROR_COLOR = "#FC5532";

        public void ChangeStateText(string text, string color = STATE_DEFAULT_COLOR)
        {
            ThisHeader.CurrentState.Text = text;
            ThisHeader.CurrentState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                ThisHeader.maximizeButton.Visibility = Visibility.Collapsed;
                ThisHeader.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                ThisHeader.maximizeButton.Visibility = Visibility.Visible;
                ThisHeader.restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.RefreshMaximizeRestoreButton();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void MyTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MyTabControl.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(connectionPage);
                    break;
                case 1:
                    MainFrame.Navigate(runPage);
                    break;
                case 2:
                    MainFrame.Navigate(frontPanelPage);
                    break;
                case 3:
                    MainFrame.Navigate(loggerPage);
                    break;
                case 4:
                    MainFrame.Navigate(cameraPage);
                    break;
                case 5:
                    MainFrame.Navigate(settingsPage);
                    break;
                case 6:
                    MainFrame.Navigate(devPage);
                    break;
                case 7:
                    MainFrame.Navigate(joyPage);
                    break;
            }
        }
    }
}
