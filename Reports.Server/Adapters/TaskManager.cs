using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Reports.Models.Model;
using Action = Reports.Models.Model.Action;

namespace Reports.Server.Adapters
{
    public class TaskManager : ITaskManager
    {
        public TaskManager()
        {
            History = new List<Action>();
            AllTasks = new List<Task>();
        }

        private List<Action> History { get; set; }
        private List<Task> AllTasks { get; set; }
        private string AllTasksJsonPath { get; set; } = @"C:\Users\DELL\Documents\log\tasks.json";
        private string HistoryJsonPath { get; set; } = @"C:\Users\DELL\Documents\log\history.json";


        public Task CreateTask(string name, Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            var newTask = new Task(name, Guid.NewGuid(), DateTime.Now, TaskState.Open); // Creating tasks with empty guid to test
            AllTasks.Add(newTask);
            History.Add(new Action(employee, newTask.Guid, DateTime.Now, $"{employee.Name} created new Task: {newTask.Name}"));
            SaveTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return newTask;
        }

        public Task FindById(Guid guid)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = AllTasks.Find(task => task.Guid.Equals(guid));
            return task;
        }

        public List<Task> FindByEmployee(Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return AllTasks.FindAll(task => task.StatedEmployee == employee).ToList();
        }
        public List<Task> FindDoneByEmployeeFromDate(Employee employee, DateTime dateTime)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return AllTasks.FindAll(task => task.StatedEmployee == employee && task.State == TaskState.Resolved && task.CreationTime>=dateTime).ToList();
        }

        public List<Task> FindCreatedFromDate(DateTime dateTime)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return AllTasks.FindAll(task => task.CreationTime >= dateTime);
        }

        public List<Task> FindChangedByEmployee(Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return History.FindAll(action => action.Employee.Equals(employee)).Select(action => action.TaskGuid).Select(FindById).ToList();
        }

        public List<Task> FindByEmployeesSubordinates(Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return (from subordinate in employee.Subordinates from task in AllTasks where task.StatedEmployee == subordinate select task).ToList();
        }

        public Task FindByCreationTime(DateTime dateTime)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = AllTasks.Find(task1 => task1.CreationTime==dateTime);
            return task;
        }

        public Task SetEmployee(Employee employeeAssigned, Guid guid, Employee employeeMadeChange)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = FindById(guid);
            task.StatedEmployee = employeeAssigned;
            task.State = TaskState.Active;
            History.Add(new Action(employeeMadeChange, task.Guid, DateTime.Now, $"Set {employeeAssigned.Name} to task {task.Name}"));
            SaveTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return task;
        }

        public Task AddComment(string comment, Guid guid, Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = FindById(guid);
            task.Comments.Add(comment);
            History.Add(new Action(employee, task.Guid, DateTime.Now, $"{employee.Name} added comment to task {task.Name}. Comment: {comment}"));
            SaveTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return task;
        }

        public Task ChangeState(TaskState taskState, Guid guid, Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = FindById(guid);
            task.State = taskState;
            History.Add(new Action(employee, task.Guid, DateTime.Now, $"Changed task {task.Name} state to {taskState.ToString()}"));
            SaveTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return task;
        }

        public List<Task> GetAll()
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return AllTasks;
        }

        public Task ChangeDescription(Guid id, string description, Employee employee)
        {
            LoadTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            Task task = FindById(id);
            task.Description = description;
            History.Add(new Action(employee, task.Guid, DateTime.Now, $"{employee.Name} changed description in task {task.Name}. Comment: {description}"));
            SaveTasksAndHistory(AllTasksJsonPath, HistoryJsonPath);
            return task;
        }

        private void SaveTasksAndHistory(string tasksPath, string historyPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(tasksPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, AllTasks);
            }
            using (StreamWriter sw = new StreamWriter(historyPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, History);
            }
        }

        private void LoadTasksAndHistory(string tasksPath, string historyPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamReader sr = new StreamReader(tasksPath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                AllTasks = serializer.Deserialize<List<Task>>(reader);
            }
            using (StreamReader sr = new StreamReader(historyPath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                History = serializer.Deserialize<List<Action>>(reader);

            }
        }
    }
}