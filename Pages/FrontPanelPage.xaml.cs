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
        }

        private void OnOutVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                OutputVariables.ItemsSource = null;
                OutputVariables.ItemsSource = InfoHolder.OutVariables;
            });
        }

        private void OnInVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                InputVariables.ItemsSource = null;
                InputVariables.ItemsSource = InfoHolder.InVariables;
            });
        }

        private void OnChartVariablesChange(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChartVariables.ItemsSource = null;
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
        bool isInDrag = false;
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
    }
}
