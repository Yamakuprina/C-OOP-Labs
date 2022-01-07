using System;
using System.Threading;

namespace IsuExtra
{
    public class Lesson
    {
        public Lesson(DateTime start, DateTime end, int auditoryNumber, string teacher)
        {
            LessonStart = start;
            LessonEnd = end;
            if (start > end)
            {
                throw new OgnpException("Invalid date");
            }

            AuditoryNumber = auditoryNumber;
            Teacher = teacher;
        }

        public Group Group { get; set; }
        public Flow Flow { get; set; }
        public DateTime LessonStart { get; set; }
        public DateTime LessonEnd { get; set; }
        private int AuditoryNumber { get; set; }
        private string Teacher { get; set; }
    }
}