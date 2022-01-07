namespace Isu
{
    public class Student
    {
        public Student(string name, Group studentGroup, CourseNumber studentCourse)
        {
            Id = ID.Generate();
            Name = name;
            Group = studentGroup;
            CourseNumber = studentCourse;
        }

        public int Id { get; }
        public string Name { get; }
        public Group Group { get; }
        public CourseNumber CourseNumber { get; }
    }
}