using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls;
using shufflecad_4.Holders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FrontPanelPage.xaml
    /// </summary>
    public partial class FrontPanelPage : Page
    {
        public FrontPanelPage()
        {
            InitializeComponent();

            InfoHolder.OnOutVariablesChange += new EventHandler<EventArgs>(OnOutVariablesChange);
            InfoHolder.OnInVariablesChange += new EventHandler<EventArgs>(OnInVariablesChange);
            InfoHolder.OnChartVariablesChange += new EventHandler<EventArgs>(OnChartVariablesChange);

            // test - remove then

            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "1" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime22", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });

            OnInVariablesChange(null, EventArgs.Empty);

            // test - remove then
        }

        private void OnOutVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                OutputVariables.ItemsSource = null;
                OutputVariables.Items.Clear();
                OutputVariables.ItemsSource = InfoHolder.OutVariables;
            });
        }

        private void OnInVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                InputVariables.ItemsSource = null;
                InputVariables.Items.Clear();
                InputVariables.ItemsSource = InfoHolder.InVariables;
            });
        }

        private void OnChartVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChartVariables.ItemsSource = null;
                ChartVariables.Items.Clear();
                ChartVariables.ItemsSource = InfoHolder.ChartVariables;
            });
        }

        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            //if (!(lastSelect is null) && !MoveToggled)
            //{
            //    SetElementOnField(lastSelect, (float)p.X, (float)p.Y);
            //}
            scrollMousePoint = e.GetPosition(null);
            hOff = scrollViewer.HorizontalOffset;
            vOff = scrollViewer.VerticalOffset;
            canvas.CaptureMouse();
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                if (!(draggingElement is null))
                {
                    var element = draggingElement;
                    //var element = sender as FrameworkElement;
                    element.ReleaseMouseCapture();
                    isInDrag = false;
                    e.Handled = true;
                    draggingElement = null;
                }
            }
            if (canvas.IsMouseCaptured)
            {
                canvas.ReleaseMouseCapture();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(null).X));
                scrollViewer.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(null).Y));
            }
        }

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
            e.Handled = true;
            draggingElement = element;

            // selectedPanelOnCanvas = sender as StackPanel;
        }

        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                //var element = sender as FrameworkElement;
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
                    //var element = sender as FrameworkElement;
                    element.ReleaseMouseCapture();
                    isInDrag = false;
                    e.Handled = true;
                    draggingElement = null;
                }
            }

        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            IFrontVariable var = e.Data.GetData(DataFormats.Serializable) as IFrontVariable;
            if (var != null)
            {
                switch (var.Type)
                {
                    case ShuffleVariable.STRING_TYPE:
                        {
                            break;
                        }
                    case ShuffleVariable.BOOL_TYPE:
                        {
                            if (var.Direction == ShuffleVariable.OUT_DIR)
                            {
                                OutBoolControl ctrl = new OutBoolControl(var as ShuffleVariable);
                                SetUpCtrl(ctrl, e);
                            }
                            else
                            {
                                InBoolControl ctrl = new InBoolControl(var as ShuffleVariable);
                                SetUpCtrl(ctrl, e);
                            }
                            break;
                        }
                    case ShuffleVariable.FLOAT_TYPE:
                        {
                            break;
                        }
                    case ShuffleVariable.CHART_TYPE:
                        {
                            break;
                        }
                    case ShuffleVariable.BIG_STRING_TYPE:
                        {
                            break;
                        }
                }
            }
        }

        private void SetUpCtrl(FrameworkElement ctrl, DragEventArgs e)
        {
            ctrl.MouseLeftButtonDown += new MouseButtonEventHandler(root_MouseLeftButtonDown);
            ctrl.MouseMove += new MouseEventHandler(root_MouseMove);
            ctrl.MouseLeftButtonUp += new MouseButtonEventHandler(root_MouseLeftButtonUp);
            Point position = e.GetPosition(canvas);
            Canvas.SetLeft(ctrl, position.X);
            Canvas.SetTop(ctrl, position.Y);
            canvas.Children.Add(ctrl);
        }

        private void InputVars_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement frameworkElement)
            {
                DragDrop.DoDragDrop(frameworkElement, new DataObject(DataFormats.Serializable, frameworkElement.DataContext), DragDropEffects.Move);
            }
        }
    }
}
