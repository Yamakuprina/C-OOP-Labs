using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.TasksServices
{
    public partial class ChangeTaskAssignedEmployee : Window
    {
        public ChangeTaskAssignedEmployee()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            UpdateTask updateTask = new UpdateTask();
            updateTask.Show();
            Hide();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            string id = TextBoxId.Text;
            string name = TextBoxEmployeeName.Text;
            Task task = ChangeAssignedEmployee(id, name, App.User.Name);
            if (task == null)
            {
                H1Changed.Text = "Cant change task";
                ChangedTask.Text = "";
            }
            else
            {
                H1Changed.Text = "Updated task: ";
                ChangedTask.Text = "NAME: " + task.Name + " ID: " + task.Guid;
            }
        }
        private static Task ChangeAssignedEmployee(string guid, string employeeName, string employeeIdent)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/tasks/assign/?guid={guid}&employeeName={employeeName}&employeeIdent={employeeIdent}");
            request.Method = WebRequestMethods.Http.Put;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Task task = JsonConvert.DeserializeObject<Task>(responseString);
            return task;
        }
    }
}