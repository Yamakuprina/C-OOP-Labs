using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.TasksServices
{
    public partial class CreateTask : Window
    {
        public CreateTask()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            TasksService tasksService = new TasksService();
            tasksService.Show();
            Hide();
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            string taskName = TextBoxTaskName.Text;
            string employeeName = TextBoxEmployeeName.Text;
            Task task = CreateNewTask(taskName, employeeName);
            if (task == null)
            {
                H1Created.Text = "Cant create task";
                CreatedTask.Text = "";
            }
            else
            {
                H1Created.Text = "Created task: ";
                CreatedTask.Text = "NAME: " + task.Name + " ID: " + task.Guid;
            }
        }
        private static Task CreateNewTask(string taskName, string employeeName)
        {
            WebRequest request =
                HttpWebRequest.Create(
                    $"https://localhost:5001/tasks/?taskName={taskName}&employeeIdent={employeeName}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Task task = JsonConvert.DeserializeObject<Task>(responseString);
            if (task == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            return task;
        }
    }
}