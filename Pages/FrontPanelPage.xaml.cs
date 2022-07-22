using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls;
using shufflecad_4.Controls.Interfaces;
using shufflecad_4.Helpers;
using shufflecad_4.Holders;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace shufflecad_4.Pages
{
    /// <summary>
    /// Interaction logic for FrontPanelPage.xaml
    /// </summary>
    public partial class FrontPanelPage : Page
    {
        public FrontPanelPage()
        {
            InitializeComponent();

            InfoHolder.OnAllVariablesChange += new EventHandler<EventArgs>(OnAllVariablesChange);

            // test - remove then

            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime1", Type = ShuffleVariable.STRING_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime2", Type = ShuffleVariable.STRING_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime3", Type = ShuffleVariable.BIG_STRING_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime4", Type = ShuffleVariable.BIG_STRING_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime5", Type = ShuffleVariable.FLOAT_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime6", Type = ShuffleVariable.FLOAT_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ChartVariable() { Name = "anime7", Type = ShuffleVariable.CHART_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime8", Type = ShuffleVariable.SLIDER_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            //InfoHolder.AllVariables.Add(new ShuffleVariable() { Name = "anime9", Type = ShuffleVariable.SLIDER_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "24" });

            //OnAllVariablesChange(null, EventArgs.Empty);

            // test - remove then
        }

        private void OnAllVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AllVariables.ItemsSource = null;
                AllVariables.Items.Clear();
                AllVariables.ItemsSource = InfoHolder.AllVariables;
            });
        }

        // moving on canvas logic

        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(null);
            hOff = scrollViewer.HorizontalOffset;
            vOff = scrollViewer.VerticalOffset;
            canvas.CaptureMouse();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(null).X));
                scrollViewer.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(null).Y));
            }
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (canvas.IsMouseCaptured)
            {
                canvas.ReleaseMouseCapture();
            }
        }

        // moving elements logic

        Point anchorPoint;
        Point currentPoint;
        bool isInDrag = true;
        FrameworkElement draggingElement;

        private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            draggingElement = element;

            e.Handled = true;
        }

        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                if (draggingElement != null)
                {
                    var element = draggingElement;
                    currentPoint = e.GetPosition(null);
                    TranslateTransform transform = new TranslateTransform();
                    transform.X += element.RenderTransform.Value.OffsetX + (currentPoint.X - anchorPoint.X);
                    transform.Y += element.RenderTransform.Value.OffsetY + (currentPoint.Y - anchorPoint.Y);
                    element.RenderTransform = transform;
                    anchorPoint = currentPoint;
                }
            }
        }

        private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                if (draggingElement != null)
                {
                    var element = draggingElement;
                    element.ReleaseMouseCapture();
                    isInDrag = false;
                    draggingElement = null;

                    e.Handled = true;
                }
            }
        }

        // drag and drop logic

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            IFrontVariable variable = e.Data.GetData(DataFormats.Serializable) as IFrontVariable;
            Point position = e.GetPosition(canvas);
            PlaceVariable(variable, position);
        }

        private void PlaceVariable(IFrontVariable variable, Point position)
        {
            if (variable != null)
            {
                FrameworkElement ctrl = ControlsFactory.GetFactory().Create(variable);
                if (ctrl != null)
                {
                    SetUpCtrl(ctrl, position);
                    // добавляем в лист переменную, которая будет находиться на панели
                    if (!InfoHolder.VariablesOnFrontPanel.Contains(variable))
                        InfoHolder.VariablesOnFrontPanel.Add(variable);
                }
            }
        }

        private void SetUpCtrl(FrameworkElement ctrl, Point position)
        {
            (ctrl as IRemoveable).OnRemove += WannaBeRemoved;

            ctrl.MouseLeftButtonDown += new MouseButtonEventHandler(root_MouseLeftButtonDown);
            ctrl.MouseMove += new MouseEventHandler(root_MouseMove);
            ctrl.MouseLeftButtonUp += new MouseButtonEventHandler(root_MouseLeftButtonUp);
            Canvas.SetLeft(ctrl, position.X);
            Canvas.SetTop(ctrl, position.Y);
            canvas.Children.Add(ctrl);
        }

        private void AllVars_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement frameworkElement)
            {
                DragDrop.DoDragDrop(frameworkElement, new DataObject(DataFormats.Serializable, frameworkElement.DataContext), DragDropEffects.Move);
            }
        }

        // buttons logic

        private void SaveFieldButton_Click(object sender, RoutedEventArgs e)
        {
            string path = FrontPanelHelper.GetSavingPath();
            if (path != null)
            {
                var vars = FrontPanelHelper.GetVariablesList(canvas);
                FrontPanelHelper.SaveVariables(path, vars);
            }
        }

        private void OpenFieldButton_Click(object sender, RoutedEventArgs e)
        {
            var vars = FrontPanelHelper.OpenSavedFile();
            if (vars != null)
            {
                FrontPanelHelper.AddVariables(vars);
                foreach (var svc in vars)
                {
                    PlaceVariable(svc.Variable, new Point(svc.XPos, svc.YPos));
                }
            }
        }

        private void CleanFieldButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            InfoHolder.VariablesOnFrontPanel.Clear();
        }

        // removing
        private void WannaBeRemoved(object sender, EventArgs args)
        {
            (sender as IRemoveable).OnRemove -= WannaBeRemoved;
            canvas.Children.Remove(sender as FrameworkElement);
            InfoHolder.VariablesOnFrontPanel.Remove((sender as IHaveVariable).GetVariable());
        }
    }
}
