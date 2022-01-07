using System.Collections.Generic;
using Isu;

namespace IsuExtra
{
    public class Group
    {
        private List<Lesson> schedule = new List<Lesson>();
        public Group(string name, MegaFaculty megaFaculty, int maxStudent = 32)
        {
            Name = name;
            CourseNumber = (CourseNumber)int.Parse(string.Empty + name[2]);
            MaxStudent = maxStudent;
            MegaFaculty = megaFaculty;
        }

        public CourseNumber CourseNumber { get; set; }
        public IReadOnlyList<Lesson> Schedule => schedule.AsReadOnly();
        public int MaxStudent { get; set; }
        public string Name { get; set; }
        public int CountStudent { get; set; } = 0;

        public MegaFaculty MegaFaculty { get; set; }

        public void AddLessonToSchedule(Lesson lesson)
        {
            schedule.Add(lesson);
        }
    }
}