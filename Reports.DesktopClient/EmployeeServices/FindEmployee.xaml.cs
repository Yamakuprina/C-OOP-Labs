using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class FindEmployee : Window
    {
        public FindEmployee()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        
        private void Find(object sender, RoutedEventArgs e)
        {
            Employee employee;
            string guid = TextBoxId.Text;
            string name = TextBoxName.Text;
            if (guid == string.Empty)
            {
                employee = FindEmployeeByName(name);
            }
            else
            {
                employee = FindEmployeeById(guid);
            }

            if (employee == null)
            {
                H1Found.Text = "Not Found";
                FoundEmployee.Text = "";
            }
            else
            {
                H1Found.Text = "Found employee: ";
                FoundEmployee.Text = "NAME: " + employee.Name + " ID: " + employee.Guid;
            }
        }
        private static Employee FindEmployeeById(string guid)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?guid={guid}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                return employee;

            }
            catch (WebException)
            {
                return null;
            }
        }

        private static Employee FindEmployeeByName(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?name={name}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                return employee;

            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}