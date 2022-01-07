using System;
using System.Collections.Generic;

namespace Reports.Models.Model
{
    public interface IReportManager
    {
        public Report CreateReport(Employee employee, List<Task> tasksDone, string description);

        public List<Report> SubordinateReportsFromDate(Employee employee, DateTime dateTime);

        public List<Employee> SubordinatesWithNoReportFromDate(Employee employee, DateTime dateTime);

        public Report FindById(Guid guid);

        public Report AddCompletedTaskToReport(Guid reportGuid, Task task);

        public Report ChangeDescription(Guid reportGuid, string description);

        public Report ChangeState(Guid reportGuid, ReportState reportState);
    }
}