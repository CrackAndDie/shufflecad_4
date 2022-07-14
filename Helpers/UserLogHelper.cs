using System;
using System.IO;

namespace shufflecad_4.Helpers
{
    public class UserLogHelper
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SoftHubSettings\\";
        private static readonly string logPath = path + @"userLogShufflecad4.txt";

        public UserLogHelper(string logMessage)
        {
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
                using (var file = File.Create(logPath))
                {

                }
            }
            LogWrite(logMessage);
        }

        public void LogWrite(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(logPath))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("-------------------------------");
                txtWriter.WriteLine(logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {

            }

        }
    }
}
