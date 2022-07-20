using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for RunPage.xaml
    /// </summary>
    public partial class RunPage : Page
    {
        public RunPage()
        {
            InitializeComponent();
        }

        private void AnyTB_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow.settingsPage.Save();
        }

        async private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            await CommandsHelper.StartProgram();
        }

        async private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await CommandsHelper.StopProgram();
        }

        private void SelectSrcFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            string existingPath = InfoHolder.CurrentSettings.PathToSrc;

            // ставим уже известный путь, если он был
            if (!string.IsNullOrEmpty(existingPath))
                dialog.SelectedPath = existingPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;

                InfoHolder.CurrentSettings.PathToSrc = selectedPath;
                PathToSrcFolderTB.Text = selectedPath;
            }
        }

        private void SelectMainFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Browse Main Py File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "py",
                Filter = "py files (*.py)|*.py",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if ((bool)openFileDialog1.ShowDialog())
            {
                string selectedFile = openFileDialog1.SafeFileName;

                InfoHolder.CurrentSettings.MainFileName = selectedFile;
                MainFileNameTB.Text = selectedFile;
            }
        }
    }
}
