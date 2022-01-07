using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class ChangeReportState : Window
    {
        public ChangeReportState()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            UpdateReport updateReport = new UpdateReport();
            updateReport.Show();
            Hide();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            string reportId = TextBoxId.Text;
            string state = TextBoxState.Text;
            Report report = ChangeState(reportId, state);
            if (report == null)
            {
                H1Changed.Text = "Cant change report";
                ChangedReport.Text = "";
            }
            else
            {
                H1Changed.Text = "Updated report state: ";
                ChangedReport.Text = "EMPLOYEE NAME: " + report.Employee.Name + " ID: " + report.Guid;
            }
        }
        private static Report ChangeState(string reportGuid, string newState)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/reports/state/?reportGuid={reportGuid}&state={newState}");
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