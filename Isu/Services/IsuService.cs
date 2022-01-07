using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private List<Student> _students = new List<Student>();
        private List<Group> _groups = new List<Group>();

        public Group AddGroup(string name)
        {
            var @group = new Group(name);
            _groups.Add(@group);
            return @group;
        }

        public Student AddStudent(Group @group, string name)
        {
            if (@group.CountStudent >= @group.MaxStudent)
            {
                throw new IsuException($"Too many students in group");
            }

            @group.CountStudent += 1;
            var newStudent = new Student(name, @group, @group.CourseNumber);
            _students.Add(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Student allstudent in _students)
            {
                if (id.Equals(allstudent.Id))
                {
                    return allstudent;
                }
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in _students)
            {
                if (name.Equals(student.Name))
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            var students = new List<Student>();
            foreach (Student student in _students)
            {
                if (FindGroup(groupName).Equals(student.Group))
                {
                    students.Add(student);
                }
            }

            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = new List<Student>();
            foreach (Student student in _students)
            {
                if (student.CourseNumber.Equals(courseNumber))
                {
                    students.Add(student);
                }
            }

            return students;
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group @group in _groups)
            {
                if (groupName.Equals(@group.Name))
                {
                    return @group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = new List<Group>();
            foreach (Group @group in _groups)
            {
                if (@group.CourseNumber.Equals(courseNumber))
                {
                    groups.Add(@group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            _students.Remove(student);
            newGroup.CountStudent -= 1;
            var newStudent = new Student(student.Name, newGroup, newGroup.CourseNumber);
            _students.Add(newStudent);
        }
    }
}