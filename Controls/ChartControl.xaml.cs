using ScottPlot;
using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for ChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl, IRemoveable, IHaveVariable
    {
        private readonly ChartVariable variable;

        List<WpfPlotViewer> wpfPlotViewers = new List<WpfPlotViewer>();

        public event EventHandler OnRemove;

        public ChartControl(ChartVariable variable)
        {
            InitializeComponent();

            this.variable = variable;
            DataContext = this.variable;

            this.variable.DataChanged += OnDataChanged;

            SetUpChart();
        }

        public IFrontVariable GetVariable()
        {
            return this.variable;
        }

        private void SetUpChart()
        {
            DataChart.Configuration.LeftClickDragPan = false;
            DataChart.Configuration.RightClickDragZoom = false;
            DataChart.Configuration.ScrollWheelZoom = false;

            DataChart.Plot.XAxis.Label(null);
            DataChart.Plot.YAxis.Label(null);

            //DataChart.Plot.XAxis.TickLabelStyle(rotation: 45);
            //DataChart.Plot.YAxis.TickLabelStyle(rotation: 45);

            DataChart.Plot.Style(ScottPlot.Style.Gray1);

            var padding = new ScottPlot.PixelPadding(
                left: 35,
                right: 7,
                bottom: 23,
                top: 7);

            DataChart.Plot.ManualDataArea(padding);

            DataChart.Plot.AddSignal(this.variable.Data);

            DataChart.Render();
        }

        private void OnDataChanged(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataChart.Plot.AxisAuto();
                DataChart.Refresh();
                DataChart.Render();

                foreach (var chart in wpfPlotViewers)
                {
                    chart.wpfPlot1.Render();
                }
            });
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var chart in wpfPlotViewers)
            {
                chart.Close();
            }
            OnRemove?.Invoke(this, EventArgs.Empty);
        }

        private void DataChart_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenNewWindow();
        }

        private void OpenNewWindow()
        {
            WpfPlotViewer newWindow = new WpfPlotViewer(DataChart.Plot)
            {
                Title = this.variable.Name
            };
            newWindow.Show();
            wpfPlotViewers.Add(newWindow);
        }

        private void NewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewWindow();
        }
    }
}
