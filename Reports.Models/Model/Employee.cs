using System;
using System.Collections.Generic;

namespace Reports.Models.Model
{
    public class Employee
    {
        public Employee(string name, Guid guid, Employee chief = default)
        {
            Name = name;
            Chief = chief;
            Subordinates = new List<Employee>();
            Guid = guid;
        }

        public string Name { get; set; }
        public Employee Chief { get; set; }
        public List<Employee> Subordinates { get; set; }
        public Guid Guid { get; set; }
    }
}