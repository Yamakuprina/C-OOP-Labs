using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Reports.Models.Model;


namespace Reports.Server.Adapters
{
    public class EmployeeManager : IEmployeeManager
    {
        private List<Employee> AllEmployee { get; set; }
        private string AllEmployeeJsonPath { get; set; } = @"C:\Users\DELL\Documents\log\employees.json";
        
        public EmployeeManager()
        {
            AllEmployee = new List<Employee>();
        }
        
        public Employee AddEmployee( string name, Employee chief = default)
        {
            LoadEmployees(AllEmployeeJsonPath);
            var employee = new Employee(name, Guid.NewGuid(), chief); // Creating all employees with same guid to test
            AllEmployee.Add(employee);
            SaveEmployees(AllEmployeeJsonPath);
            return employee;
        }

        public Employee FindById(Guid guid)
        {
            LoadEmployees(AllEmployeeJsonPath);
            Employee employee = AllEmployee.Find(employee => employee.Guid.Equals(guid));
            return employee;
        }

        public Employee FindByName(string name)
        {
            LoadEmployees(AllEmployeeJsonPath);
            Employee employee = AllEmployee.Find(employee1 => employee1.Name.Equals(name));
            return employee;
        }

        public Employee ChangeChief(string chiefName, string subordinateName)
        {
            LoadEmployees(AllEmployeeJsonPath);
            Employee chief = FindByName(chiefName);
            Employee subordinate = FindByName(subordinateName);
            subordinate.Chief = chief;
            chief.Subordinates.Add(subordinate);
            SaveEmployees(AllEmployeeJsonPath);
            return subordinate;
        }

        public void DeleteEmployee(string name)
        {
            LoadEmployees(AllEmployeeJsonPath);
            Employee employee = AllEmployee.Find(employee => employee.Name.Equals(name));
            AllEmployee.Remove(employee);
            SaveEmployees(AllEmployeeJsonPath);
        }

        public List<Employee> GetAllEmployees()
        {
            LoadEmployees(AllEmployeeJsonPath);
            return AllEmployee;
        }
        
        private void SaveEmployees(string employeesPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(employeesPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, AllEmployee);
            }
        }

        private void LoadEmployees(string employeesPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamReader sr = new StreamReader(employeesPath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                AllEmployee = serializer.Deserialize<List<Employee>>(reader);
            }
        }
    }
}