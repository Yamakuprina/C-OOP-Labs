using System.Windows;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class EmployeeService : Window
    {
        public EmployeeService()
        {
            InitializeComponent();
        }
        
        private void Go_back(object sender, RoutedEventArgs e)
        {
            Tabs tabs = new Tabs();
            tabs.Show();
            Hide();
        }
        
        private void Get_All(object sender, RoutedEventArgs e)
        {
            GetAll getAll = new GetAll();
            getAll.Show();
            Hide();
        }

        private void Find_Employee(object sender, RoutedEventArgs e)
        {
            FindEmployee findEmployee = new FindEmployee();
            findEmployee.Show();
            Hide();
        }
        
        private void Update_Employee(object sender, RoutedEventArgs e)
        {
            UpdateEmployee updateEmployee = new UpdateEmployee();
            updateEmployee.Show();
            Hide();
        }
        
        private void Delete_Employee(object sender, RoutedEventArgs e)
        {
            DeleteEmployee deleteEmployee = new DeleteEmployee();
            deleteEmployee.Show();
            Hide();
        }
        
        private void Add_Employee(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
            Hide();
        }
    }
}