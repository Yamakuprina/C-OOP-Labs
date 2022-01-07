using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class GetAll : Window
    {
        private int start_counter = 0;
        public GetAll()
        {
            InitializeComponent();
            EmployeesList.ItemsSource = GetAllEmployeesFirstPage();
        }
        
        private void Go_back(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        
        private void Next_page(object sender, RoutedEventArgs e)
        {
            start_counter += 10;
            EmployeesList.ItemsSource = GetAllEmployeesNextPage(start_counter.ToString());
        }
        
        private void Prev_page(object sender, RoutedEventArgs e)
        {
            if (start_counter >= 10)
            {
                start_counter -= 10;
            }
            EmployeesList.ItemsSource = GetAllEmployeesNextPage(start_counter.ToString());
        }
        
        private static List<Employee> GetAllEmployeesFirstPage()
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/all/");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
            if (employees == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }
            if (employees.Count > 0)
            {
                return employees;
            }
            return null;
        }
        
        private static List<Employee> GetAllEmployeesNextPage(string start)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/all/next-page/?start={start}");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
            if (employees == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            if (employees.Count > 0)
            {
                return employees;
            }

            return null;
        }
    }
}