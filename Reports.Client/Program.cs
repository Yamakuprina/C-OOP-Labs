using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
using Reports.Server;
using Reports.Models.Model;

namespace Reports.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate(object sender, X509Certificate certificate, X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
            Console.WriteLine("Log into your account:");
            Employee user = default;
            while (true)
            {
                Console.WriteLine("Enter your Id: ");
                user = Identify(Console.ReadLine());
                if (user != default) break;
                    else continue;
            }
            Console.WriteLine($"Hello, {user.Name}!");
            while (true)
            {
                Console.WriteLine("\nChoose a tab:\n" +
                                  "1. Employees\n" +
                                  "2. Tasks\n" +
                                  "3. Reports\n" +
                                  "4. Exit\n");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("\nPick option:\n" +
                                          "1. Get all employees\n" +
                                          "2. Find employee\n" +
                                          "3. Update employee\n" +
                                          "4. Delete employee\n" +
                                          "5. Add new employee\n" +
                                          "6. Go back\n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("First page:");
                                GetAllEmployeesFirstPage();
                                Console.WriteLine("Enter number of the page");
                                switch (Console.ReadLine())
                                {
                                    case"2":
                                        Console.WriteLine("Second page:");
                                        GetAllEmployeesNextPage(10.ToString());
                                        break;
                                    case "3":
                                        Console.WriteLine("Third page:");
                                        GetAllEmployeesNextPage(20.ToString());
                                        break;
                                }
                                break;
                            case "2":
                                Console.WriteLine("\nPick option:\n" +
                                                  "1. Find employee with Id\n" +
                                                  "2. Find employee with Name\n" +
                                                  "3. Go back\n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Enter employee Id:");
                                        FindEmployeeById(Console.ReadLine());
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter employee name:");
                                        FindEmployeeByName(Console.ReadLine());
                                        break;
                                    case "3":
                                        break;
                                }
                                break;
                            case "3":
                                Console.WriteLine("\nPick option:\n" +
                                                  "1. Change employee's chief\n" +
                                                  "2. Go back\n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Enter employee name:");
                                        string employeeName = Console.ReadLine();
                                        Console.WriteLine("Enter chief name:");
                                        string chiefName = Console.ReadLine();
                                        ChangeEmployeesChief(employeeName, chiefName);
                                        break;
                                    case "2":
                                        break;
                                }

                                break;
                            case "4":
                                Console.WriteLine("Enter employee name:");
                                DeleteEmployee(Console.ReadLine());
                                break;
                            case "5":
                                Console.WriteLine("Enter employee name:");
                                CreateEmployee(Console.ReadLine());
                                break;
                            case "6":
                                break;
                        }
                        break;
                    case "2":
                        Console.WriteLine("\nPick option:\n" +
                                          "1. Get all tasks\n" +
                                          "2. Find task\n" +
                                          "3. Create task\n" +
                                          "4. Update task\n" +
                                          "5. Go back\n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                GetAllTasks();
                                break;
                            case "2":
                                Console.WriteLine("\nPick option:\n" +
                                                  "1. Find task by Id\n" +
                                                  "2. Find task by creation time\n" +
                                                  "3. Find tasks by assigned employee\n" +
                                                  "4. Find tasks changed by employee\n" +
                                                  "5. Find tasks assigned to employee's subordinates\n" +
                                                  "6. Go back\n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Enter task Id:");
                                        FindTaskById(Console.ReadLine());
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter creation time:");
                                        FindTaskByCreationTime(Console.ReadLine());
                                        break;
                                    case "3":
                                        Console.WriteLine("Enter employee name:");
                                        FindTasksByEmployeeAssigned(Console.ReadLine());
                                        break;
                                    case "4":
                                        Console.WriteLine("Enter employee name:");
                                        FindTasksChangedByEmployee(Console.ReadLine());
                                        break;
                                    case "5":
                                        Console.WriteLine("Enter employee name:");
                                        FindTasksByEmployeeSubordinates(Console.ReadLine());
                                        break;
                                    case "6":
                                        break;
                                }
                                break;
                            case "3":
                                Console.WriteLine("Enter task name:");
                                CreateTask(Console.ReadLine(), user.Name);
                                break;
                            case "4":
                                Console.WriteLine("\nPick option:\n" +
                                                  "1. Change task state\n" +
                                                  "2. Add comment to task\n" +
                                                  "3. Change task assigned employee\n" +
                                                  "4. Change task description\n" +
                                                  "5. Go back\n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Enter task Id:");
                                        string taskId = Console.ReadLine();
                                        Console.WriteLine("Enter new task state:");
                                        string taskState = Console.ReadLine();
                                        ChangeTaskState(taskId, taskState, user.Name);
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter task Id:");
                                        string taskGuid = Console.ReadLine();
                                        Console.WriteLine("Enter task comment:");
                                        string comment = Console.ReadLine();
                                        AddTaskComment(taskGuid, comment, user.Name);
                                        break;
                                    case "3":
                                        Console.WriteLine("Enter task Id:");
                                        string taskid = Console.ReadLine();
                                        Console.WriteLine("Enter assigned employee's name:");
                                        string name = Console.ReadLine();
                                        ChangeTaskAssignedEmployee(taskid, name, user.Name);
                                        break;
                                    case "4":
                                        Console.WriteLine("Enter task Id:");
                                        string taskguid = Console.ReadLine();
                                        Console.WriteLine("Enter new description:");
                                        string description = Console.ReadLine();
                                        ChangeTaskDescription(taskguid, description, user.Name);
                                        break;
                                    case "5":
                                        break;
                                }
                                break;
                            case "5":
                                break;
                        }
                        break;
                    case "3":
                        Console.WriteLine("\nPick option:\n" +
                                          "1. Create weekly report\n" +
                                          "2. Get tasks on this week\n" +
                                          "3. Get subordinates reports\n" +
                                          "4. Get subordinates without reports\n" +
                                          "5. Add completed task to report\n" +
                                          "6. Update report\n" +
                                          "7. Go back\n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("Enter description:");
                                CreateWeeklyReport(Console.ReadLine(), user.Name);
                                break;
                            case "2":
                                GetTasksCreatedWeekly();
                                break;
                            case "3":
                                Console.WriteLine("Enter employee name:");
                                GetSubordinatesReportsWeekly(Console.ReadLine());
                                break;
                            case "4":
                                Console.WriteLine("Enter employee name:");
                                GetSubordinatesWithNoReportsWeekly(Console.ReadLine());
                                break;
                            case "5":
                                Console.WriteLine("Enter task Id:");
                                string taskId = Console.ReadLine();
                                Console.WriteLine("Enter report Id:");
                                string reportId = Console.ReadLine();
                                AddTaskToReport(reportId, taskId);
                                break;
                            case "6":
                                Console.WriteLine("\nPick option:\n" +
                                                  "1. Change report state\n" +
                                                  "2. Update description\n" +
                                                  "3. Go back\n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Enter new state:");
                                        string state = Console.ReadLine();
                                        Console.WriteLine("Enter report Id:");
                                        string repGuId = Console.ReadLine();
                                        ChangeReportState(repGuId, state);
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter new description:");
                                        string description = Console.ReadLine();
                                        Console.WriteLine("Enter report Id:");
                                        string reportGuId = Console.ReadLine();
                                        ChangeReportDescription(reportGuId, description);
                                        break;
                                    case "3":
                                        break;
                                }

                                break;
                            case "7":
                                break;
                        }
                        break;
                    case "4":
                        return;
                }
            }
        }

        private static void CreateEmployee(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?name={name}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
            if (employee == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine("Created employee:");
            Console.WriteLine($"Id: {employee.Guid}  Name: {employee.Name}");
        }

        private static Employee Identify(string guid)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?guid={guid}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                if (employee == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }
                return employee;
            }
            catch (WebException e)
            {
                Console.WriteLine("Wrong login: " + guid);
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }

        private static void FindEmployeeById(string guid)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?guid={guid}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                if (employee == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                Console.WriteLine("Found employee by Id:");
                Console.WriteLine($"Id: {employee.Guid}  Name: {employee.Name}");

            }
            catch (WebException e)
            {
                Console.WriteLine("Employee not found: " + guid);
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void FindEmployeeByName(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?name={name}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                if (employee == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                Console.WriteLine("Found employee by Name:");
                Console.WriteLine($"Id: {employee.Guid}  Name: {employee.Name}");

            }
            catch (WebException e)
            {
                Console.WriteLine("Employee not found");
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void GetAllEmployeesFirstPage()
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/all/");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
            if (employees == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            if (employees.Count > 0)
            {
                Console.WriteLine("All employees:");
                for (int i = 0; i < employees.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Id: {employees[i].Guid}  Name: {employees[i].Name}");
                }
            }
            else Console.WriteLine("No employees found");
        }
        
        private static void GetAllEmployeesNextPage(string start)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/all/next-page/?start={start}");
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
            if (employees == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            if (employees.Count > 0)
            {
                Console.WriteLine("All employees:");
                for (int i = 0; i < employees.Count; i++)
                {
                    Console.WriteLine($"{start + i + 1}. Id: {employees[i].Guid}  Name: {employees[i].Name}");
                }
            }
            else Console.WriteLine("No employees found");
        }

        private static void DeleteEmployee(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/delete/?name={name}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            var status = JsonConvert.DeserializeObject<HttpStatusCode>(responseString);
            Console.WriteLine("Deleted successfully: " + status.ToString());
        }

        private static void ChangeEmployeesChief(string employeeName, string chiefName)
        {
            WebRequest request =
                HttpWebRequest.Create(
                    $"https://localhost:5001/employees/?employeeName={employeeName}&chiefName={chiefName}");
            request.Method = WebRequestMethods.Http.Put;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
            if (employee == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine("Updated employee:");
            Console.WriteLine($"Id: {employee.Guid}  Name: {employee.Name} ");
        }

        private static void CreateTask(string taskName, string employeeName)
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

            Console.WriteLine("Created task:");
            Console.WriteLine($"Name: {task.Name} Id: {task.Guid} ");
        }

        private static void GetAllTasks()
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

            if (tasks.Count > 0)
            {
                Console.WriteLine("All tasks:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Name: {tasks[i].Name} Id: {tasks[i].Guid}");
                }
            }
            else Console.WriteLine("No tasks yet");

        }

        private static void FindTaskById(string id)
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
                if (task == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                Console.WriteLine("Found task by Id:");
                Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");

            }
            catch (WebException e)
            {
                Console.WriteLine("Task not found: " + id);
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void FindTasksByEmployeeAssigned(string employeeName)
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
                if (tasks == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                if (tasks.Count > 0)
                {
                    Console.WriteLine($"Found tasks assigned to employee: {employeeName}:");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Name: {tasks[i].Name} Id: {tasks[i].Guid}");
                    }
                }
                else Console.WriteLine("No tasks found");
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void FindTasksChangedByEmployee(string employeeName)
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
                if (tasks == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                if (tasks.Count > 0)
                {
                    Console.WriteLine($"Found tasks changed by employee: {employeeName}:");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Name: {tasks[i].Name} Id: {tasks[i].Guid}");
                    }
                }
                else Console.WriteLine("No tasks found");
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void ChangeTaskState(string guid, string taskState, string employeeIdent)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/tasks/state/?guid={guid}&taskState={taskState}&employeeIdent={employeeIdent}");
            request.Method = WebRequestMethods.Http.Put;
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

            Console.WriteLine("Changed task state:");
            Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");
        }

        private static void AddTaskComment(string guid, string comment, string employeeIdent)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/tasks/comment/?guid={guid}&comment={comment}&employeeIdent={employeeIdent}");
            request.Method = WebRequestMethods.Http.Put;
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

            Console.WriteLine($"Added comment '{comment}' to task:");
            Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");
        }

        private static void ChangeTaskAssignedEmployee(string guid, string employeeName, string employeeIdent)
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
            if (task == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine($"Assigned {employeeName} to task:");
            Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");
        }

        private static void FindTasksByEmployeeSubordinates(string employeeName)
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
                if (tasks == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                if (tasks.Count > 0)
                {
                    Console.WriteLine($"Found tasks assigned to {employeeName} subordinates:");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Name: {tasks[i].Name} Id: {tasks[i].Guid}");
                    }
                }
                else Console.WriteLine("No tasks found");
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void ChangeTaskDescription(string guid, string description, string employeeIdent)
        {
            WebRequest request = HttpWebRequest.Create(
                $"https://localhost:5001/tasks/description/?guid={guid}&description={description}&employeeIdent={employeeIdent}");
            request.Method = WebRequestMethods.Http.Put;
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

            Console.WriteLine($"Changed description '{description}' in task:");
            Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");
        }

        private static void FindTaskByCreationTime(string dateTime)
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
                if (task == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                Console.WriteLine("Found task by Creation time:");
                Console.WriteLine($"Name: {task.Name} Id: {task.Guid}");

            }
            catch (WebException e)
            {
                Console.WriteLine("Task not found: " + dateTime);
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void CreateWeeklyReport(string description, string employeeIdent)
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
            if (report == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine("Created report:");
            Console.WriteLine($"Employee: {report.Employee.Name} Id: {report.Guid} Description: {report.Description} ");
        }

        private static void GetTasksCreatedWeekly()
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/reports/");
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

            if (tasks.Count > 0)
            {
                Console.WriteLine($"Tasks done this week:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Name: {tasks[i].Name} Id: {tasks[i].Guid} State: {tasks[i].State}");
                }
            }
            else Console.WriteLine("No tasks done this week");
        }

        private static void GetSubordinatesReportsWeekly(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/reports/subordinates/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString);
                if (reports == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                if (reports.Count > 0)
                {
                    Console.WriteLine($"Reports of {employeeName} subordinates:");
                    for (int i = 0; i < reports.Count; i++)
                    {
                        Console.WriteLine(
                            $"{i + 1}. Employee: {reports[i].Employee.Name} State: {reports[i].ReportState} Description: {reports[i].Description}");
                    }
                }
                else Console.WriteLine("No reports found");
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
        
        private static void GetSubordinatesWithNoReportsWeekly(string employeeName)
        {
            try
            {
                WebRequest request =
                    HttpWebRequest.Create($"https://localhost:5001/reports/subordinates/no-reports/?employeeName={employeeName}");
                request.Method = WebRequestMethods.Http.Get;
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
                if (employees == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }

                if (employees.Count > 0)
                {
                    Console.WriteLine($"Subordinates of {employeeName} without reports:");
                    for (int i = 0; i < employees.Count; i++)
                    {
                        Console.WriteLine(
                            $"{i + 1}. Employee: {employees[i].Name}");
                    }
                }
                else Console.WriteLine("No subordinates without reports found");
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
        
        private static void AddTaskToReport(string reportGuid, string taskGuid)
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
            if (report == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine($"Added task '{taskGuid}' to report:");
            Console.WriteLine($"Employee: {report.Employee.Name} Id: {report.Guid} Description: {report.Description} ");
        }
        
        private static void ChangeReportDescription(string reportGuid, string description)
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
            if (report == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine($"Changed description '{description}' in report:");
            Console.WriteLine($"Employee: {report.Employee.Name} Id: {report.Guid} Description: {report.Description} ");
        }
        
        private static void ChangeReportState(string reportGuid, string newState)
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
            if (report == null)
            {
                throw new ReportsException("Object cant be deserialized");
            }

            Console.WriteLine($"Changed state to '{newState}' in report:");
            Console.WriteLine($"Employee: {report.Employee.Name} Id: {report.Guid} Description: {report.Description} ");
        }
    }
}