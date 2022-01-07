using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.TasksServices
{
    public partial class GetAll : Window
    {
        public GetAll()
        {
            InitializeComponent();
            TasksList.ItemsSource = GetAllTasks();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            TasksService tasksService = new TasksService();
            tasksService.Show();
            Hide();
        }
        
        private static List<Task> GetAllTasks()
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/tasks/all/");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
            if (tasks == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            return tasks;
        }
    }
}