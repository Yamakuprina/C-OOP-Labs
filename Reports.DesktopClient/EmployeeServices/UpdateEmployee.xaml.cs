using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class UpdateEmployee : Window
    {
        public UpdateEmployee()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            string employeeName = TextBoxEmployeeName.Text;
            string chiefName = TextBoxChiefName.Text;
            Employee employee = ChangeEmployeesChief(employeeName, chiefName);
            if (employee == null)
            {
                H1Update.Text = "Cant update employee";
            }
            else
            {
                H1Update.Text = "Updated employee: ";
                UpdatedEmployee.Text = "NAME: " + employee.Name + " ID: " + employee.Guid;
            }
        }
        private static Employee ChangeEmployeesChief(string employeeName, string chiefName)
        {
            WebRequest request =
                HttpWebRequest.Create(
                    $"https://localhost:5001/employees/?employeeName={employeeName}&chiefName={chiefName}");
            request.Method = WebRequestMethods.Http.Put;
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