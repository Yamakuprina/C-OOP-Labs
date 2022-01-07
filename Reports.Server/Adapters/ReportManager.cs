using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Reports.Models.Model;

namespace Reports.Server.Adapters
{
    public class ReportManager : IReportManager
    {
        private List<Report> AllReports { get; set; }
        private string AllReportsJsonPath { get; set; } = @"C:\Users\DELL\Documents\log\reports.json";

        public ReportManager()
        {
            AllReports = new List<Report>();
        }

        public Report CreateReport(Employee employee, List<Task> tasksDone, string description)
        {
            LoadReports(AllReportsJsonPath);
            Report report = new Report(employee, DateTime.Now, tasksDone.Select(task => task.Guid).ToList(), description, ReportState.Active, Guid.NewGuid());
            AllReports.Add(report); // Creating reports with empty guid to test
            SaveReports(AllReportsJsonPath);
            return report;
        }

        public List<Report> SubordinateReportsFromDate(Employee employee, DateTime dateTime)
        {
            LoadReports(AllReportsJsonPath);
            return AllReports.FindAll(report => employee.Subordinates.Contains(report.Employee) && report.CreationTime>= dateTime);
        }

        public List<Employee> SubordinatesWithNoReportFromDate(Employee employee, DateTime dateTime)
        {
            LoadReports(AllReportsJsonPath);
            List<Employee> SubordinatesWithReport = AllReports.FindAll(report => employee.Subordinates.Contains(report.Employee) && report.CreationTime>= dateTime).Select(report => report.Employee).ToList();
            return employee.Subordinates.Where(subordinate => !SubordinatesWithReport.Contains(subordinate)).ToList();
        }

        public Report FindById(Guid guid)
        {
            LoadReports(AllReportsJsonPath);
            return AllReports.Find(report => report.Guid == guid);
        }

        public Report AddCompletedTaskToReport(Guid reportGuid, Task task)
        {
            LoadReports(AllReportsJsonPath);
            Report report = FindById(reportGuid);
            report.TasksCompleted.Add(task.Guid);
            SaveReports(AllReportsJsonPath);
            return report;
        }

        public Report ChangeDescription(Guid reportGuid, string description)
        {
            LoadReports(AllReportsJsonPath);
            Report report = FindById(reportGuid);
            report.Description = description;
            SaveReports(AllReportsJsonPath);
            return report;
        }
        
        public Report ChangeState(Guid reportGuid, ReportState reportState)
        {
            LoadReports(AllReportsJsonPath);
            Report report = FindById(reportGuid);
            report.ReportState = reportState;
            SaveReports(AllReportsJsonPath);
            return report;
        }
        
        private void SaveReports(string reportsPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(reportsPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, AllReports);
            }
        }

        private void LoadReports(string reportsPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamReader sr = new StreamReader(reportsPath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                AllReports = serializer.Deserialize<List<Report>>(reader);
            }
        }
    }
}