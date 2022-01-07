using System;
using System.Collections.Generic;

namespace Reports.Models.Model
{
    public class Report
    {
        public Report(Employee employee, DateTime creationTime, List<Guid> tasksCompleted, string description, ReportState reportState, Guid guid)
        {
            Employee = employee;
            CreationTime = creationTime;
            TasksCompleted = tasksCompleted;
            Description = description;
            ReportState = reportState;
            Guid = guid;
        }

        public Employee Employee { get; set; }
        public DateTime CreationTime { get; set; }
        public List<Guid> TasksCompleted { get; set; }
        public string Description { get; set; }
        public ReportState ReportState { get; set; }
        public Guid Guid { get; set; }
    }
}