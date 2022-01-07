using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class CreateWeeklyReport : Window
    {
        public CreateWeeklyReport()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            string description = TextBoxDescription.Text;
            Report report = CreateWeekly(description, App.User.Name);
            if (report == null)
            {
                H1Created.Text = "Cant create report";
                CreatedReport.Text = "";
            }
            else
            {
                H1Created.Text = "Created report: ";
                CreatedReport.Text = "NAME: " + report.Employee.Name + " ID: " + report.Guid;
            }
        }
        private static Report CreateWeekly(string description, string employeeIdent)
        {
            WebRequest request =
                HttpWebRequest.Create(
                    $"https://localhost:5001/reports/?description={description}&employeeIdent={employeeIdent}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Report report = JsonConvert.DeserializeObject<Report>(responseString);
            return report;
        }
    }
}