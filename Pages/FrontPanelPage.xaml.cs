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
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime1", Type = ShuffleVariable.STRING_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime2", Type = ShuffleVariable.STRING_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime3", Type = ShuffleVariable.BIG_STRING_TYPE, Direction = ShuffleVariable.OUT_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime4", Type = ShuffleVariable.BIG_STRING_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });
            InfoHolder.InVariables.Add(new ShuffleVariable() { Name = "anime5", Type = ShuffleVariable.BOOL_TYPE, Direction = ShuffleVariable.IN_DIR, Value = "0" });

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
                switch (variable.Type)
                {
                    case ShuffleVariable.STRING_TYPE:
                        {
                            if (variable.Direction == ShuffleVariable.OUT_DIR)
                            {
                                OutStringControl ctrl = new OutStringControl(variable as ShuffleVariable);
                                SetUpCtrl(ctrl, position);
                            }
                            else
                            {
                                InStringControl ctrl = new InStringControl(variable as ShuffleVariable);
                                SetUpCtrl(ctrl, position);
                            }
                            break;
                        }
                    case ShuffleVariable.BOOL_TYPE:
                        {
                            BoolControl ctrl = new BoolControl(variable as ShuffleVariable);
                            SetUpCtrl(ctrl, position);
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
                            if (variable.Direction == ShuffleVariable.OUT_DIR)
                            {
                                OutBigStringControl ctrl = new OutBigStringControl(variable as ShuffleVariable);
                                SetUpCtrl(ctrl, position);
                            }
                            else
                            {
                                InBigStringControl ctrl = new InBigStringControl(variable as ShuffleVariable);
                                SetUpCtrl(ctrl, position);
                            }
                            break;
                        }
                }
            }
        }

        private void SetUpCtrl(FrameworkElement ctrl, Point position)
        {
            ctrl.MouseLeftButtonDown += new MouseButtonEventHandler(root_MouseLeftButtonDown);
            ctrl.MouseMove += new MouseEventHandler(root_MouseMove);
            ctrl.MouseLeftButtonUp += new MouseButtonEventHandler(root_MouseLeftButtonUp);
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
