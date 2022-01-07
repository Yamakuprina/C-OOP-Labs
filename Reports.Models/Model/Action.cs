using System;

namespace Reports.Models.Model
{
    public class Action
    {
        public Employee Employee { get; set; }
        public Guid TaskGuid { get; set; }
        public DateTime DateTime { get; set; }
        public string ChangeDescribed { get; set; }

        public Action(Employee employee, Guid taskGuid, DateTime dateTime, string changeDescribed)
        {
            Employee = employee;
            TaskGuid = taskGuid;
            DateTime = dateTime;
            ChangeDescribed = changeDescribed;
        }
    }
}