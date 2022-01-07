using System.Dynamic;
using Isu;
namespace IsuExtra
{
    public class Student
    {
        public Student(string name, Group studentGroup, CourseNumber studentCourse, MegaFaculty megaFaculty)
        {
            Id = ID.Generate();
            Name = name;
            Group = studentGroup;
            CourseNumber = studentCourse;
            MegaFaculty = megaFaculty;
        }

        public int Id { get; }
        public string Name { get; }
        public Group Group { get; }
        public CourseNumber CourseNumber { get; }

        public Ognp Ognp1 { get; internal set; } = null;
        public Flow Flow1 { get; set; }
        public Ognp Ognp2 { get; internal set; } = null;
        public Flow Flow2 { get; set; }
        public MegaFaculty MegaFaculty { get; set; }
    }
}