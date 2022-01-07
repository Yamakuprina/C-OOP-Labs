using System.Windows;
using Reports.DesktopClient.EmployeeServices;
using Reports.DesktopClient.ReportsServices;
using Reports.DesktopClient.TasksServices;

namespace Reports.DesktopClient
{
    public partial class Tabs : Window
    {
        public Tabs()
        {
            InitializeComponent();
            Greeting.Text = "Hello, " + App.User.Name + "!";
        }

        private void Employees_OnClick(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        
        private void Tasks_OnClick(object sender, RoutedEventArgs e)
        {
            TasksService tasksService = new TasksService();
            tasksService.Show();
            Hide();
        }
        
        private void Reports_OnClick(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
    }
}