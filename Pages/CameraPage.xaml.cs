using shufflecad_4.Holders;
using System;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        public CameraPage()
        {
            InitializeComponent();

            InfoHolder.OnCameraVariablesChange += new EventHandler<EventArgs>(OnCameraVariablesChange);
        }

        private void OnCameraVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CamerasList.ItemsSource = null;
                CamerasList.ItemsSource = InfoHolder.CameraVariables;
            });
        }
    }
}
