using System.Windows;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class UpdateReport : Window
    {
        public UpdateReport()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
        private void ChangeReportState(object sender, RoutedEventArgs e)
        {
            ChangeReportState changeReportState = new ChangeReportState();
            changeReportState.Show();
            Hide();
        }
        private void UpdateReportDescription(object sender, RoutedEventArgs e)
        {
            UpdateReportDescription updateReportDescription = new UpdateReportDescription();
            updateReportDescription.Show();
            Hide();
        }
    }
}