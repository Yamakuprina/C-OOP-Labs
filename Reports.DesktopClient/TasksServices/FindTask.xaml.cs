using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.TasksServices
{
    public partial class FindTask : Window
    {
        public FindTask()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            TasksService tasksService = new TasksService();
            tasksService.Show();
            Hide();
        }
        
        private void Find(object sender, RoutedEventArgs e)
        {
            List<Task> tasksFound = new List<Task>();
            string Id = TextBoxId.Text;
            string creationTime = TextBoxCreationTime.Text;
            string assignedEmployee = TextBoxAssignedEmployee.Text;
            string changedByEmployee = TextBoxChangedByEmployee.Text;
            string subordinatesAssigned = TextBoxSubordinatesAssigned.Text;
            if (Id != string.Empty)
            {
                tasksFound.Add(FindTaskById(Id));
            }
            if (assignedEmployee != string.Empty)
            {
                tasksFound.AddRange(FindTasksByEmployeeAssigned(assignedEmployee));
            }
            if (changedByEmployee != string.Empty)
            {
                tasksFound.AddRange(FindTasksChangedByEmployee(changedByEmployee));
            }
            if (subordinatesAssigned != string.Empty)
            {
                tasksFound.AddRange(FindTasksByEmployeeSubordinates(subordinatesAssigned));
            }
            if (creationTime != string.Empty)
            {
                tasksFound.Add(FindTaskByCreationTime(creationTime));
            }
            if (tasksFound.Count>0)
            {
                H1Found.Text = "Found tasks: ";
                TasksList.ItemsSource = tasksFound;
            }
            else
            {
                H1Found.Text = "Tasks not found";
                TasksList.ItemsSource = null;
            }
        }
        private static Task FindTaskById(string id)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/tasks/id/?guid={id}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                return task;
            }
            catch (WebException)
            {
                return null;
            }
        }

        private static List<Task> FindTasksByEmployeeAssigned(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/tasks/assigned/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                return tasks;
            }
            catch (WebException)
            {
                return null;
            }
        }

        private static List<Task> FindTasksChangedByEmployee(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/tasks/changed/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                return tasks;
            }
            catch (WebException)
            {
                return null;
            }
        }
        
        private static List<Task> FindTasksByEmployeeSubordinates(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/tasks/subordinates/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                return tasks;
            }
            catch (WebException)
            {
                return null;
            }
        }
        private static Task FindTaskByCreationTime(string dateTime)
        {
            WebRequest request =
                HttpWebRequest.Create($"https://localhost:5001/tasks/time/?dateTime={dateTime}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                return task;

            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}