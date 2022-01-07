using System;
using System.Collections.Generic;

namespace Reports.Models.Model
{
    public interface ITaskManager
    {
        public Task CreateTask(string name, Employee employee);

        public Task FindById(Guid guid);
        public List<Task> FindByEmployee(Employee employee);
        public List<Task> FindChangedByEmployee(Employee employee);
        public List<Task> FindByEmployeesSubordinates(Employee employee);

        public Task FindByCreationTime(DateTime dateTime);
        public List<Task> FindDoneByEmployeeFromDate(Employee employee, DateTime dateTime);
        public List<Task> FindCreatedFromDate(DateTime dateTime);

        public Task SetEmployee(Employee employeeAssigned, Guid guid, Employee employeeMadeChange);

        public Task AddComment(string comment, Guid guid, Employee employee);

        public Task ChangeState(TaskState taskState, Guid guid, Employee employee);
        public List<Task> GetAll();
        public Task ChangeDescription(Guid id, string description, Employee employee);

    }
}