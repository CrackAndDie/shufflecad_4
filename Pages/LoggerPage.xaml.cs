using shufflecad_4.Holders;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

            if ((bool)ScrollDownButton.IsChecked)
            {
                LoggerBox.ScrollToEnd();
            }
        }

        public void SafeAppendTextToLog(string text, string color)
        {
            // безопасность вызовов из других потоков
            Application.Current.Dispatcher.Invoke(() =>
            {
                AppendTextToLog(text, color);
            });
        }

        public void AppendTextToLog(string text)
        {
            // alice blue
            AppendTextToLog(text, "#F0F8FF");
        }

        public void SafeAppendTextToLog(string text)
        {
            // безопасность вызовов из других потоков
            Application.Current.Dispatcher.Invoke(() =>
            {
                AppendTextToLog(text);
            });
        }
    }
}
