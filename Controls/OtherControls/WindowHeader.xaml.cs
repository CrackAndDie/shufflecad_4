using shufflecad_4.Classes;
using shufflecad_4.Helpers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls.OtherControls
{
    /// <summary>
    /// Interaction logic for WindowHeader.xaml
    /// </summary>
    public partial class WindowHeader : UserControl
    {
        public WindowHeader()
        {
            InitializeComponent();
        }

        public void InitLangCB(List<LanguageClass> langs)
        {
            LanguageCB.ItemsSource = langs;
            LanguageCB.SelectedIndex = 0;
        }

        public void InitDone()
        {
            ProgressAsyncPB.Visibility = Visibility.Collapsed;
            AllDonePI.Visibility = Visibility.Visible;
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.ThisMainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ThisMainWindow.WindowState = WindowState.Maximized;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ThisMainWindow.WindowState = WindowState.Normal;
        }

        async private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            await CommandsHelper.StopChecks();
            await ConnectionHelper.StopHelper();
            await JoystickHelper.StopJoystick();
            MainWindow.ThisMainWindow.Close();
        }
    }
}
