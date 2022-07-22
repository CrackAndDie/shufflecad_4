using Microsoft.Win32;
using shufflecad_4.Classes;
using shufflecad_4.Classes.Variables;
using shufflecad_4.Controls.Interfaces;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace shufflecad_4.Helpers
{
    public class FrontPanelHelper
    {
        // save file
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

        // open saved file
        public static List<SavingVariableClass> OpenSavedFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                if (File.Exists(path))
                {
                    string txt = File.ReadAllText(path);
                    try
                    {
                        return ConvertToNormalVariablesList(JsonSerializer.Deserialize<List<OpenSavingVariableClass>>(txt));
                    }
                    catch (JsonException)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public static void AddVariables(List<SavingVariableClass> vars)
        {
            foreach (SavingVariableClass svc in vars)
            {
                var inAll = InfoHolder.AllVariables.FirstOrDefault(x => x.Name == svc.Variable.Name);
                if (inAll != null)
                {
                    svc.Variable = inAll;
                }
                else
                {
                    InfoHolder.AllVariables.Add(svc.Variable);
                }
            }
            InfoHolder.OnAllVariablesChangeWrapper();
        }

        // cringe method, idk how to do it better
        private static List<SavingVariableClass> ConvertToNormalVariablesList(List<OpenSavingVariableClass> vars)
        {
            List<SavingVariableClass> outList = new List<SavingVariableClass>();
            foreach (var v in vars)
            {
                var svc = new SavingVariableClass();
                svc.XPos = v.XPos;
                svc.YPos = v.YPos;
                if (v.Variable.Type == ShuffleVariable.CHART_TYPE)
                {
                    svc.Variable = new ChartVariable(v.Variable);
                }
                else
                {
                    svc.Variable = new ShuffleVariable(v.Variable);
                }

                outList.Add(svc);
            }

            return outList;
        }
    }
}
