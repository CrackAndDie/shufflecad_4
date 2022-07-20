using Microsoft.Win32;
using shufflecad_4.Classes;
using shufflecad_4.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Helpers
{
    public class FrontPanelHelper
    {
        public static List<SavingVariableClass> GetVariablesList(Canvas canvas)
        {
            List<SavingVariableClass> variables = new List<SavingVariableClass>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(canvas); i++)
            {
                var child = VisualTreeHelper.GetChild(canvas, i);

                if (child != null)
                {
                    SavingVariableClass sv = new SavingVariableClass();
                    var castedVar = (child as IHaveVariable).GetVariable();
                    sv.Variable = castedVar;
                    sv.XPos = (float)Canvas.GetLeft(child as UIElement);
                    sv.YPos = (float)Canvas.GetTop(child as UIElement);
                    variables.Add(sv);
                }
            }
            return variables;
        }

        public static string GetSavingPath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json file (*.json)|*.json";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.ValidateNames = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        public static void SaveVariables(string path, List<SavingVariableClass> variables)
        {
            string json = JsonSerializer.Serialize<List<SavingVariableClass>>(variables);
            File.WriteAllText(path, json);
        }
    }
}
