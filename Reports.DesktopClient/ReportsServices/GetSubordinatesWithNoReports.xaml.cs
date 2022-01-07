using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class GetSubordinatesWithNoReports : Window
    {
        public GetSubordinatesWithNoReports()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
        private void Get(object sender, RoutedEventArgs e)
        {
            string chiefName = TextBoxName.Text;
            List<Employee> employees = GetSubordinatesWithNoReportsWeekly(chiefName);
            if (employees == null)
            {
                H1Found.Text = "No subordinates found";
                EmployeesList.ItemsSource = null;
            }
            else
            {
                H1Found.Text = "Found subordinates: " + employees.Count;
                EmployeesList.ItemsSource = employees;
            }
        }
        private static List<Employee> GetSubordinatesWithNoReportsWeekly(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/reports/subordinates/no-reports/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
                return employees;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}