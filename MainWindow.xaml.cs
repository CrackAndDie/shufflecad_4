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

            LanguageCB.ItemsSource = languageClasses;
            LanguageCB.SelectedIndex = 0;

            connectionPage = new ConnectionPage();
            runPage = new RunPage();
            frontPanelPage = new FrontPanelPage();
            loggerPage = new LoggerPage();
            cameraPage = new CameraPage();
            devPage = new DevPage();

            settingsPage = new SettingsPage();
            settingsPage.LoadAndSet();

            CommandsHelper.StartChecks();
            ConnectionHelper.StartHelper();
            JoystickHelper.StartJoystick();

            ConnectionHelper.OnRPIDataChange += new EventHandler<EventArgs>(InfoHolder.CurrentRPIData.OnRPIDataChange);

            loggerPage.AppendTextToLog("Shufflecad inited", "#00AEAE");

            ProgressAsyncPB.Visibility = Visibility.Collapsed;
            AllDonePI.Visibility = Visibility.Visible;
        }

        public const string STATE_DEFAULT_COLOR = "#F0F8FF";
        public const string STATE_WARNING_COLOR = "#DEAA1D";
        public const string STATE_ERROR_COLOR = "#FC5532";

        public void ChangeStateText(string text, string color = STATE_DEFAULT_COLOR)
        {
            CurrentState.Text = text;
            CurrentState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        async private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            await CommandsHelper.StopChecks();
            await ConnectionHelper.StopHelper();
            await JoystickHelper.StopJoystick();
            this.Close();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
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
            }
        }
    }
}
