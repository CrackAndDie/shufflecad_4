using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.settingsPage.Save();
        }

        private void FixButton_Click(object sender, RoutedEventArgs e)
        {

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
