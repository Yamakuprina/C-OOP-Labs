using System;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        public Group(string name, int maxStudent = 32)
        {
            if (!Validator.IsGroupNameCorrect(name))
            {
                throw new IsuException($"Incorrect Group Name : {name}");
            }

            Name = name;
            CourseNumber = (CourseNumber)int.Parse(string.Empty + name[2]);
            MaxStudent = maxStudent;
        }

        public CourseNumber CourseNumber { get; }
        public int MaxStudent { get; }
        public string Name { get; }
        public int CountStudent { get; set; } = 0;
    }
}