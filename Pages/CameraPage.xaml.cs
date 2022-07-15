using shufflecad_4.Classes.Variables;
using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        private CameraVariable currentCameraVariable;
        public CameraPage()
        {
            InitializeComponent();

            InfoHolder.OnCameraVariablesChange += new EventHandler<EventArgs>(OnCameraVariablesChange);
            ConnectionHelper.OnDisconnect += new EventHandler<EventArgs>(OnDisconnect);
        }

        private void OnCameraVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CamerasList.ItemsSource = null;
                CamerasList.Items.Clear();
                CamerasList.ItemsSource = InfoHolder.CameraVariables;
            });
        }

        async private void OnImageUpdateAsync(object sender, PropertyChangedEventArgs args)
        {
            if (currentCameraVariable != null)
            {
                if (args.PropertyName == "Value")
                {
                    await SetImage(currentCameraVariable.Value);
                }
            }
        }

        private void OnDisconnect(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindow.cameraPage.ImageCamera.Source = null;
            });
        }

        async private Task SetImage(byte[] array)
        {
            try
            {
                
                await Task.Run(() =>
                {
                    var image = new BitmapImage();
                    using (var mem = new MemoryStream(array))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow.cameraPage.ImageCamera.Source = image;
                    });
                });
                
            }
            catch (Exception ex)
            {
                InfoHolder.UserLogger.LogWrite(ex.ToString());
            }
        }

        private void CamerasList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InfoHolder.CameraVariablesAllGot)
            {
                if (CamerasList.SelectedItem != null)
                {
                    if (currentCameraVariable != null)
                    {
                        currentCameraVariable.PropertyChanged -= OnImageUpdateAsync;
                    }

                    InfoHolder.CurrentSelectedCamera = CamerasList.SelectedIndex;
                    currentCameraVariable = CamerasList.SelectedItem as CameraVariable;
                    currentCameraVariable.PropertyChanged += OnImageUpdateAsync;
                }
            }
            else
            {
                CamerasList.SelectedItem = null;
            }
        }

        private void ImageCamera_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(ImageCamera);

            int newWidth = 0;
            int newHeight = 0;
            if (currentCameraVariable != null)
            {
                int imageW = int.Parse(currentCameraVariable.Shape[0]);
                int imageH = int.Parse(currentCameraVariable.Shape[1]);
                newWidth = (int)((p.X / ImageCamera.ActualWidth) * imageW);
                newHeight = (int)((p.Y / ImageCamera.ActualHeight) * imageH);
            }

            MouseCoordsTB.Text = $"{newWidth};{newHeight}";
        }
    }
}
