using System;
using System.Collections.Generic;


namespace Reports.Models.Model
{
    public interface IEmployeeManager
    {
        public Employee AddEmployee(string name, Employee chief = default);

        public Employee FindById(Guid guid);
        public Employee FindByName(string name);

        public Employee ChangeChief(string chiefName, string subordinateName);
        public void DeleteEmployee(string name);
        public List<Employee> GetAllEmployees();
    }
}