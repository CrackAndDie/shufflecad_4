using shufflecad_4.Classes;
using shufflecad_4.Holders;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SoftHubSettings\\";
        private static readonly string settingsPath = path + @"settingsShufflecad4.json";

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void AnyButton_LostFocus(object sender, RoutedEventArgs e)
        {
            Save();
        }

        public void LoadAndSet()
        {
            if (File.Exists(settingsPath))
            {
                string txt = File.ReadAllText(settingsPath);
                SettingsClass loadedSaveObject = JsonSerializer.Deserialize<SettingsClass>(txt);
                SetSetts(loadedSaveObject);
            }
            else
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Save();
            }
        }

        private void SetSetts(SettingsClass setts)
        {
            DetailedOutputButton.IsChecked = setts.DetailedOutput;
            ShowMinimapButton.IsChecked = setts.ShowMinimap;

            MainWindow.runPage.PathToSrcFolderTB.Text = setts.PathToSrc;
            MainWindow.runPage.MainFileNameTB.Text = setts.MainFileName;

            MainWindow.runPage.IpAddressTB.Text = setts.IpAddress;
            MainWindow.runPage.UserNameTB.Text = setts.UserName;
            MainWindow.runPage.PasswordTB.Text = setts.Password;

            ApplySetts(setts);
        }

        private void ApplySetts(SettingsClass setts)
        {
            InfoHolder.CurrentSettings = setts;

            ChangeCommandFiles(InfoHolder.CurrentSettings);

            if (setts.ShowMinimap)
            {
                MainWindow.frontPanelPage.MinimapBorder.Visibility = Visibility.Visible;
            }
            else MainWindow.frontPanelPage.MinimapBorder.Visibility = Visibility.Hidden;
        }

        public void Save()
        {
            // берем текущее окно
            MainWindow mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            mainWindow.ChangeStateText("Settings saved");

            SettingsClass setts = new SettingsClass()
            {
                DetailedOutput = (bool)DetailedOutputButton.IsChecked,
                ShowMinimap = (bool)ShowMinimapButton.IsChecked,

                PathToSrc = MainWindow.runPage.PathToSrcFolderTB.Text,
                MainFileName = MainWindow.runPage.MainFileNameTB.Text,

                IpAddress = MainWindow.runPage.IpAddressTB.Text,
                UserName = MainWindow.runPage.UserNameTB.Text,
                Password = MainWindow.runPage.PasswordTB.Text,
            };

            ApplySetts(setts);

            string json = JsonSerializer.Serialize(setts);
            File.WriteAllText(settingsPath, json);
        }

        private void ChangeCommandFiles(SettingsClass data)
        {
            string text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/StartProgramSample.txt");
            text = text.Replace("userNameHere", data.UserName);
            text = text.Replace("userPasswordHere", data.Password);
            text = text.Replace("ipAddressHere", data.IpAddress);
            text = text.Replace("distDirHere", data.PathToSrc);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/StartProgram.txt", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/StopProgramSample.txt");
            text = text.Replace("userNameHere", data.UserName);
            text = text.Replace("userPasswordHere", data.Password);
            text = text.Replace("ipAddressHere", data.IpAddress);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/StopProgram.txt", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/CheckIsRunningSample.txt");
            text = text.Replace("userNameHere", data.UserName);
            text = text.Replace("userPasswordHere", data.Password);
            text = text.Replace("ipAddressHere", data.IpAddress);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/CheckIsRunning.txt", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/LoadCommandsSample.txt");
            text = text.Replace("userNameHere", data.UserName);
            text = text.Replace("userPasswordHere", data.Password);
            text = text.Replace("ipAddressHere", data.IpAddress);
            text = text.Replace("currentPath", Directory.GetCurrentDirectory());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/LoadCommands.txt", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/StartPyFileSample.sh");
            text = text.Replace("pathToPyHere", "/home/pi/pycad/" + data.MainFileName);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/StartPyFile.sh", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/StopPyFileSample.sh");
            text = text.Replace("pyNameHere", data.MainFileName.Split('.')[0]);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/StopPyFile.sh", text);

            text = File.ReadAllText(Directory.GetCurrentDirectory() + "/PyCommands/ProgramIsRunningSample.sh");
            text = text.Replace("programNameHere", data.MainFileName.Split('.')[0]);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/PyCommands/ProgramIsRunning.sh", text);
        }
    }
}
