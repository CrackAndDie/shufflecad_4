using shufflecad_4.Classes.Variables;
using System;
using System.Windows;
using System.Windows.Controls;

namespace shufflecad_4.Controls
{
    /// <summary>
    /// Interaction logic for ChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl
    {
        private readonly ChartVariable varaible;

        public ChartControl(ChartVariable varaible)
        {
            InitializeComponent();

            this.varaible = varaible;
            DataContext = this.varaible;

            this.varaible.DataChanged += OnDataChanged;

            SetUpChart();
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
                left: 25,
                right: 10,
                bottom: 25,
                top: 10);

            DataChart.Plot.ManualDataArea(padding);

            DataChart.Plot.AddSignal(this.varaible.Data);

            DataChart.Render();
        }

        private void OnDataChanged(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataChart.Plot.AxisAuto();
                DataChart.Refresh();
                DataChart.Render();
            });
        }
    }
}
