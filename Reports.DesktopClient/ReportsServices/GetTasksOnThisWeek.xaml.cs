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
    public partial class GetTasksOnThisWeek : Window
    {
        public GetTasksOnThisWeek()
        {
            InitializeComponent();
            TasksList.ItemsSource = GetTasksCreatedWeekly();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            ReportsService reportsService = new ReportsService();
            reportsService.Show();
            Hide();
        }
        private static List<Task> GetTasksCreatedWeekly()
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/reports/");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
            return tasks;
        }
    }
}