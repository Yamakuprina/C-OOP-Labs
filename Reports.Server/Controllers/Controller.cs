using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Reports.Models.Model;

namespace Reports.Server.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {
        private IEmployeeManager _employeeManager;
        private ITaskManager _taskManager;
        private IReportManager _reportManager;
        
        public Controller(IEmployeeManager employeeManager, ITaskManager taskManager, IReportManager reportManager)
        {
            _employeeManager = employeeManager;
            _taskManager = taskManager;
            _reportManager = reportManager;
        }
        
        [HttpPost]
        [Route("employees")]
        public Employee CreateEmployee([FromQuery] string name)
        {
            return _employeeManager.AddEmployee(name); 
        }

        [HttpGet]
        [Route("employees")]
        public IActionResult Find([FromQuery] string name, [FromQuery] Guid guid)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Employee result = _employeeManager.FindByName(name);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            if (guid != Guid.Empty)
            {
                Employee result = _employeeManager.FindById(guid);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Route("employees/delete")]
        public HttpStatusCode DeleteEmployee([FromQuery] string name)
        {
            _employeeManager.DeleteEmployee(name);
            return HttpStatusCode.OK;
        }

        [HttpGet]
        [Route("employees/all")]
        public List<Employee> GetAllEmployeesFirstPage()
        {
            if (_employeeManager.GetAllEmployees().Count < 10) return _employeeManager.GetAllEmployees();
            return _employeeManager.GetAllEmployees().Take(10).ToList();
        }
        
        [HttpGet]
        [Route("employees/all/next-page")]
        public List<Employee> GetAllEmployeesNextPage([FromQuery] int start)
        {
            if (_employeeManager.GetAllEmployees().Count < start) return new List<Employee>();
            if (_employeeManager.GetAllEmployees().Count < start + 10) return _employeeManager.GetAllEmployees().Skip(start).ToList();
            return _employeeManager.GetAllEmployees().Skip(start).Take(10).ToList();
        }

        [HttpPut]
        [Route("employees")]
        public Employee ChangeChief([FromQuery] string employeeName, [FromQuery] string chiefName)
        {
            return _employeeManager.ChangeChief(chiefName, employeeName);
        }

        [HttpPost]
        [Route("tasks")]
        public Task CreateTask([FromQuery] string taskName, [FromQuery] string employeeIdent)
        {
            Employee employee = _employeeManager.FindByName(employeeIdent);
            return _taskManager.CreateTask(taskName, employee);
        }
        
        [HttpGet]
        [Route("tasks/all")]
        public List<Task> GetAllTasks()
        {
            return _taskManager.GetAll();
        }
        
        [HttpGet]
        [Route("tasks/time")]
        public Task FindByCreationTime([FromQuery] DateTime dateTime)
        {
            return _taskManager.FindByCreationTime(dateTime);
        }

        [HttpGet]
        [Route("tasks/id")]
        public Task FindTaskById([FromQuery] Guid guid)
        {
            return _taskManager.FindById(guid);
        }

        [HttpGet]
        [Route("tasks/assigned")]
        public List<Task> FindTasksByEmployeeAssigned([FromQuery] string employeeName)
        {
            Employee employee = _employeeManager.FindByName(employeeName);
            return _taskManager.FindByEmployee(employee);
        }

        [HttpGet]
        [Route("tasks/changed")]
        public List<Task> FindTasksChangedByEmployee([FromQuery] string employeeName)
        {
            Employee employee = _employeeManager.FindByName(employeeName);
            return _taskManager.FindChangedByEmployee(employee);
        }

        [HttpPut]
        [Route("tasks/state")]
        public Task ChangeTaskState([FromQuery] Guid guid, [FromQuery] TaskState taskState, [FromQuery] string employeeIdent)
        {
            Employee employee = _employeeManager.FindByName(employeeIdent);
            return _taskManager.ChangeState(taskState, guid, employee);
        }
        
        [HttpPut]
        [Route("tasks/comment")]
        public Task AddTaskComment([FromQuery] Guid guid, [FromQuery] string comment, [FromQuery] string employeeIdent)
        {
            Employee employee = _employeeManager.FindByName(employeeIdent);
            return _taskManager.AddComment(comment, guid, employee);
        }

        [HttpPut]
        [Route("tasks/assign")]
        public Task ChangeTaskAssignedEmployee([FromQuery] Guid guid, [FromQuery] string employeeName, [FromQuery] string employeeIdent)
        {
            Employee employeeIdentified = _employeeManager.FindByName(employeeIdent);
            Employee employee = _employeeManager.FindByName(employeeName);
            return _taskManager.SetEmployee(employee, guid, employeeIdentified);
        }

        [HttpGet]
        [Route("tasks/subordinates")]
        public List<Task> FindTasksByEmployeeSubordinates([FromQuery] string employeeName)
        {
            Employee employee = _employeeManager.FindByName(employeeName);
            return _taskManager.FindByEmployeesSubordinates(employee);
        }

        [HttpPut]
        [Route("tasks/description")]
        public Task ChangeTaskDescription([FromQuery] Guid guid, [FromQuery] string description, [FromQuery] string employeeIdent)
        {
            Employee employee = _employeeManager.FindByName(employeeIdent);
            return _taskManager.ChangeDescription(guid, description, employee);
        }

        [HttpPost]
        [Route("reports")]
        public Report CreateWeekly([FromQuery] string description, [FromQuery] string employeeIdent)
        {
            Employee employee = _employeeManager.FindByName(employeeIdent);
            List<Task> tasksDone = _taskManager.FindDoneByEmployeeFromDate(employee, DateTime.Today.AddDays(-7));
            return _reportManager.CreateReport(employee, tasksDone, description);
        }

        [HttpGet]
        [Route("reports")]
        public List<Task> GetTasksCreatedWeekly()
        {
            return _taskManager.FindCreatedFromDate(DateTime.Today.AddDays(-7));
        }

        [HttpGet]
        [Route("reports/subordinates")]
        public List<Report> GetSubordinatesReportsWeekly([FromQuery] string employeeName)
        {
            Employee employee = _employeeManager.FindByName(employeeName);
            return _reportManager.SubordinateReportsFromDate(employee, DateTime.Today.AddDays(-7));
        }

        [HttpGet]
        [Route("reports/subordinates/no-reports")]
        public List<Employee> GetSubordinatesWithNoReportsWeekly([FromQuery] string employeeName)
        {
            Employee employee = _employeeManager.FindByName(employeeName);
            return _reportManager.SubordinatesWithNoReportFromDate(employee, DateTime.Today.AddDays(-7));
        }

        [HttpPut]
        [Route("reports/add-task")]
        public Report AddTaskToReport([FromQuery] Guid reportGuid, [FromQuery] Guid taskGuid)
        {
            Task task = _taskManager.FindById(taskGuid);
            return _reportManager.AddCompletedTaskToReport(reportGuid, task);
        }

        [HttpPut]
        [Route("reports/description")]
        public Report ChangeReportDescription([FromQuery] Guid reportGuid, [FromQuery] string description)
        {
            return _reportManager.ChangeDescription(reportGuid, description);
        }

        [HttpPut]
        [Route("reports/state")]
        public Report ChangeReportState([FromQuery] Guid reportGuid, [FromQuery] ReportState state)
        {
            return _reportManager.ChangeState(reportGuid, state);
        }
    }
}