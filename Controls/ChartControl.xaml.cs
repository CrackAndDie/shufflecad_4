using shufflecad_4.Classes.Variables;
using shufflecad_4.Classes.Variables.Interfaces;
using shufflecad_4.Controls.Interfaces;
using System;
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
                left: 25,
                right: 10,
                bottom: 25,
                top: 10);

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
            });
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            OnRemove?.Invoke(this, EventArgs.Empty);
        }
    }
}
