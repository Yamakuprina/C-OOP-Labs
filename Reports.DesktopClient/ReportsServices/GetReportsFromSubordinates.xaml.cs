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
    public partial class GetReportsFromSubordinates : Window
    {
        public GetReportsFromSubordinates()
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
            List<Report> reports =GetSubordinatesReportsWeekly(chiefName);
            if (reports == null)
            {
                H1Found.Text = "No reports found";
                ReportsList.ItemsSource = null;
            }
            else
            {
                H1Found.Text = "Found reports: " + reports.Count;
                ReportsList.ItemsSource = reports;
            }
        }
        private static List<Report> GetSubordinatesReportsWeekly(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/reports/subordinates/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString);
                return reports;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}