using System;
using System.Collections.Generic;

namespace Reports.Models.Model
{
    public class Task
    {
        public Task(string name, Guid guid, DateTime creationTime, TaskState state)
        {
            Name = name;
            Guid = guid;
            CreationTime = creationTime;
            State = state;
        }

        public Employee StatedEmployee { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        public string Description { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreationTime { get; set; }
        public TaskState State { get; set; }
    }
}