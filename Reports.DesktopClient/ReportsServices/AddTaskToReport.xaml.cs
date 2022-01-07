using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class AddTaskToReport : Window
    {
        public AddTaskToReport()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            string reportId = TextBoxReportId.Text;
            string taskId = TextBoxTaskId.Text;
            Report report = AddTaskInReport(reportId, taskId);
            if (report == null)
            {
                H1Added.Text = "Cant change report";
                ChangedReport.Text = "";
            }
            else
            {
                H1Added.Text = "Added task in report: ";
                ChangedReport.Text = "EMPLOYEE NAME: " + report.Employee.Name + " ID: " + report.Guid;
            }
        }
        private static Report AddTaskInReport(string reportGuid, string taskGuid)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/reports/add-task/?reportGuid={reportGuid}&taskGuid={taskGuid}");
            request.Method = WebRequestMethods.Http.Put;
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