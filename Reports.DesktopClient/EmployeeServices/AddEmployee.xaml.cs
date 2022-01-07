using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class AddEmployee : Window
    {
        public AddEmployee()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            string name = TextBoxName.Text;
            Employee employee = CreateEmployee(name);
            H1Created.Text = "Created employee: ";
            CreatedEmployee.Text = "NAME: " + employee.Name + " ID: " + employee.Guid;
        }
        
        private static Employee CreateEmployee(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?name={name}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
            if (employee == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            return employee;
        }
    }
}