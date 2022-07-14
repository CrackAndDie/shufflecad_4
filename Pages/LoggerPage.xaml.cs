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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for LoggerPage.xaml
    /// </summary>
    public partial class LoggerPage : Page
    {
        public LoggerPage()
        {
            InitializeComponent();
        }

        private void ScrollDownButton_Checked(object sender, RoutedEventArgs e)
        {
            LoggerBox.ScrollToEnd();
        }

        public void AppendTextToLog(string text, string color)
        {
            TextRange rangeOfText = new TextRange(LoggerBox.Document.ContentEnd, LoggerBox.Document.ContentEnd);
            rangeOfText.Text = text + "\n";
            try
            {
                rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)));
            }
            catch (Exception)
            {
                if (InfoHolder.CurrentSettings.DetailedOutput)
                    AppendTextToLog("Invalid text color - use HEX like #xxxxxx");
            }
            // безопасность вызовов из других потоков
            Application.Current.Dispatcher.Invoke(() =>
            {
                if ((bool)ScrollDownButton.IsChecked)
                {
                    LoggerBox.ScrollToEnd();
                }
            });
        }

        public void AppendTextToLog(string text)
        {
            // alice blue
            AppendTextToLog(text, "#F0F8FF");
        }
    }
}
