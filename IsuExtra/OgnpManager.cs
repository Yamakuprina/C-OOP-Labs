using System;
using System.Collections.Generic;
using System.Linq;

namespace IsuExtra
{
    public class OgnpManager
    {
        private List<Student> allStudents = new List<Student>();
        private List<MegaFaculty> allMegaFacultys = new List<MegaFaculty>();

        public Group AddGroup(string name, MegaFaculty groupMegaFaculty)
        {
            var @group = new Group(name, groupMegaFaculty);
            MegaFaculty megaFaculty = allMegaFacultys.FirstOrDefault(f => f.Equals(groupMegaFaculty)) ??
                                      throw new OgnpException("Invalid MegaFaculty");
            megaFaculty.AddGroup(@group);
            return @group;
        }

        public Student AddStudent(Group @group, string name)
        {
            if (@group == null)
            {
                throw new OgnpException($"Invalid group");
            }

            if (@group.CountStudent >= @group.MaxStudent)
            {
                throw new OgnpException($"Too many students in group");
            }

            var newStudent = new Student(name, @group, @group.CourseNumber, @group.MegaFaculty);
            allStudents.Add(newStudent);
            @group.CountStudent += 1;
            return newStudent;
        }

        public Ognp AddOgnp(MegaFaculty ognpMegaFaculty, string name)
        {
            var newOgnp = new Ognp(ognpMegaFaculty, name);
            MegaFaculty megaFaculty = allMegaFacultys.FirstOrDefault(f => f.Name.Equals(ognpMegaFaculty.Name)) ??
                                      throw new OgnpException("Invalid MegaFaculty");
            megaFaculty.Ognp = newOgnp;
            return newOgnp;
        }

        public void AddMegaFaculty(MegaFaculty megaFaculty)
        {
            allMegaFacultys.Add(megaFaculty);
        }

        public bool CheckLessonCollision(Lesson lesson1, Lesson lesson2)
        {
            if (lesson1.LessonStart > lesson1.LessonEnd || lesson2.LessonStart > lesson2.LessonEnd)
                throw new OgnpException("Invalid date");

            if (lesson1.LessonStart == lesson1.LessonEnd || lesson2.LessonStart == lesson2.LessonEnd)
                return false;

            if (lesson1.LessonStart == lesson2.LessonStart || lesson1.LessonEnd == lesson2.LessonEnd)
                return true;

            if (lesson1.LessonStart < lesson2.LessonStart)
            {
                if (lesson1.LessonEnd > lesson2.LessonStart && lesson1.LessonEnd < lesson2.LessonEnd)
                    return true;

                if (lesson1.LessonEnd > lesson2.LessonEnd)
                    return true;
            }
            else
            {
                if (lesson2.LessonEnd > lesson1.LessonStart && lesson2.LessonEnd < lesson1.LessonEnd)
                    return true;

                if (lesson2.LessonEnd > lesson1.LessonEnd)
                    return true;
            }

            return false;
        }

        public bool CheckScheduleCollision(Group @group, Flow flow)
        {
            foreach (Lesson groupLesson in @group.Schedule)
            {
                foreach (Lesson flowLesson in flow.Schedule)
                {
                    if (CheckLessonCollision(groupLesson, flowLesson))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddStudentToOgnp(Ognp ognp, Student student, Flow choosedFlow)
        {
            if (student.MegaFaculty == ognp.MegaFaculty)
            {
                throw new OgnpException("Students MegaFaculty equals OGNP MegaFaculty");
            }

            if (student.Ognp1 != null && student.Ognp2 != null)
            {
                throw new OgnpException("Student already has two OGNP");
            }

            if (student.Ognp1 == null)
            {
                student.Ognp1 = ognp;
                foreach (Flow flow in ognp.AllFlows)
                {
                    if (flow.Equals(choosedFlow) && !CheckScheduleCollision(student.Group, choosedFlow))
                    {
                        student.Flow1 = choosedFlow;
                    }
                }

                return;
            }

            if (student.Ognp2 == null)
            {
                student.Ognp2 = ognp;
                foreach (Flow flow in ognp.AllFlows)
                {
                    if (flow.Equals(choosedFlow) && !CheckScheduleCollision(student.Group, choosedFlow))
                    {
                        student.Flow2 = choosedFlow;
                    }
                }

                return;
            }
        }

        public void RemoveStudentOgnp(Ognp ognp, Student student)
        {
            if (student.Ognp1 == ognp)
            {
                student.Ognp1 = null;
                student.Flow1 = null;
            }

            if (student.Ognp2 == ognp)
            {
                student.Ognp2 = null;
                student.Flow2 = null;
            }
        }

        public List<Flow> GetFlowsWithCourseNumber(CourseNumber courseNumber)
        {
            return (from megaFaculty in allMegaFacultys
                let ognp = megaFaculty.Ognp
                from flow in megaFaculty.Ognp.AllFlows
                where @flow.CourseNumber.Equals(courseNumber)
                select flow).ToList();
        }

        public List<Student> GetStudentsInOgnpFlow(Flow @flow)
        {
            return allStudents.Where(student => student.Flow1 == @flow || student.Flow2 == @flow).ToList();
        }

        public List<Student> GetStudentsWithNoOgnp(Group @group)
        {
            return allStudents.Where(student =>
                student.Group.Equals(@group) && (student.Flow1 == null || student.Flow2 == null)).ToList();
        }
    }
}