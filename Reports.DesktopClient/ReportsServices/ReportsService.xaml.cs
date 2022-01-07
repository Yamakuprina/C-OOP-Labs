using System.Windows;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class ReportsService : Window
    {
        public ReportsService()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            Tabs tabs = new Tabs();
            tabs.Show();
            Hide();
        }
        private void CreateWeeklyReport(object sender, RoutedEventArgs e)
        {
            CreateWeeklyReport createWeeklyReport = new CreateWeeklyReport();
            createWeeklyReport.Show();
            Hide();
        }
        private void GetTasksOnWeek(object sender, RoutedEventArgs e)
        {
            GetTasksOnThisWeek getTasksOnThisWeek = new GetTasksOnThisWeek();
            getTasksOnThisWeek.Show();
            Hide();
        }
        private void GetSubordinatesReports(object sender, RoutedEventArgs e)
        {
            GetReportsFromSubordinates getReportsFromSubordinates = new GetReportsFromSubordinates();
            getReportsFromSubordinates.Show();
            Hide();
        }
        private void GetSubordinatesWithNoReports(object sender, RoutedEventArgs e)
        {
            GetSubordinatesWithNoReports getSubordinatesWithNoReports = new GetSubordinatesWithNoReports();
            getSubordinatesWithNoReports.Show();
            Hide();
        }
        private void AddTaskToReport(object sender, RoutedEventArgs e)
        {
            AddTaskToReport addTaskToReport = new AddTaskToReport();
            addTaskToReport.Show();
            Hide();
        }
        private void UpdateReport(object sender, RoutedEventArgs e)
        {
            UpdateReport updateReport = new UpdateReport();
            updateReport.Show();
            Hide();
        }
    }
}