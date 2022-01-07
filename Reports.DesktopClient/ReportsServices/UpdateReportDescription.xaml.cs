using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.ReportsServices
{
    public partial class UpdateReportDescription : Window
    {
        public UpdateReportDescription()
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
            string description = TextBoxDescription.Text;
            Report report = ChangeReportDescription(reportId, description);
            if (report == null)
            {
                H1Changed.Text = "Cant change report";
                ChangedReport.Text = "";
            }
            else
            {
                H1Changed.Text = "Updated report description: ";
                ChangedReport.Text = "EMPLOYEE NAME: " + report.Employee.Name + " ID: " + report.Guid;
            }
        }
        private static Report ChangeReportDescription(string reportGuid, string description)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/reports/description/?reportGuid={reportGuid}&description={description}");
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