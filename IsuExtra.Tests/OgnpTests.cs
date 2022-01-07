using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using IsuExtra;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class OgnpTests
    {
        private OgnpManager _ognpManager;
        private MegaFaculty _megaFaculty;
        private Ognp ognp;

        [SetUp]
        public void Setup()
        {
            _ognpManager = new OgnpManager();
            _megaFaculty = new MegaFaculty("ITIP");
            _ognpManager.AddMegaFaculty(_megaFaculty);
            ognp = _ognpManager.AddOgnp(_megaFaculty, "CyberSafety");
            
        }

        [Test]
        public void AddNewOgnpInMegaFaculty()
        {
            Assert.IsNotNull(_megaFaculty.Ognp);
        }

        [Test]
        public void AddStudentToOgnpExceptionExpected()
        {
            Flow flaw = ognp.AddFlow("B12");
            Group group = _ognpManager.AddGroup("M3206", _megaFaculty);
            Student student = _ognpManager.AddStudent(group, "Ivan Ivanov");
            Assert.Catch<OgnpException>((() =>
            {
                _ognpManager.AddStudentToOgnp(ognp, student, flaw);
            }));
        }

        [Test]
        public void DeleteStudentFromOgnp()
        {
            var megaFaculty2 = new MegaFaculty("Not ITIP");
            _ognpManager.AddMegaFaculty(megaFaculty2);
            Flow flow = ognp.AddFlow("B12");
            Group group = _ognpManager.AddGroup("M3206", megaFaculty2);
            group.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,10,0,0),
                new DateTime(2021,9,1,11,30,0),325,"Stepan Stepanov" ));
            flow.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,13,30,0),
                new DateTime(2021,9,1,15,0,0),325,"Stepan Stepanov" ));
            Student student = _ognpManager.AddStudent(group, "Ivan Ivanov");
            _ognpManager.AddStudentToOgnp(ognp, student, flow);
             Ognp savedOgnp = student.Ognp1;
            _ognpManager.RemoveStudentOgnp(ognp,student);
            Assert.AreNotEqual(savedOgnp, student.Ognp1);
        }

        [Test]
        public void GetFlowsWithCourseNumber()
        {
            var megaFaculty2 = new MegaFaculty("Not ITIP");
            _ognpManager.AddMegaFaculty(megaFaculty2);
            Ognp ognp1 = _ognpManager.AddOgnp(_megaFaculty, "CyberSafety");
            Flow flow1 = ognp1.AddFlow("B12");
            Flow flow2 = ognp1.AddFlow("B13");
            Ognp ognp2 = _ognpManager.AddOgnp(megaFaculty2, "CyberSafety");
            Flow flow3 = ognp2.AddFlow("A12");
            Flow flow4 = ognp2.AddFlow("A13");
            CourseNumber courseNumber = flow1.CourseNumber;
            var flows = new List<Flow>() {flow1, flow2, flow3, flow4};
            Assert.AreEqual(flows, _ognpManager.GetFlowsWithCourseNumber(courseNumber));
        }
        [Test]
        public void GetStudentsInOgnpFlow()
        {
            var megaFaculty2 = new MegaFaculty("Not ITIP");
            _ognpManager.AddMegaFaculty(megaFaculty2);
            Group group = _ognpManager.AddGroup("M3206", megaFaculty2);
            Flow flow = ognp.AddFlow("B12");
            group.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,10,0,0),
                new DateTime(2021,9,1,11,30,0),325,"Stepan Stepanov" ));
            flow.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,13,30,0),
                new DateTime(2021,9,1,15,0,0),325,"Stepan Stepanov" ));
            Student student1 = _ognpManager.AddStudent(group, "Ivan Ivanov");
            Student student2 = _ognpManager.AddStudent(group, "Petr Petrov");
            Student student3 = _ognpManager.AddStudent(group, "Matvey Matveev");
            _ognpManager.AddStudentToOgnp(ognp, student1, flow);
            _ognpManager.AddStudentToOgnp(ognp, student2, flow);
            var students = new List<Student>() {student1, student2};
            Assert.AreEqual(students, _ognpManager.GetStudentsInOgnpFlow(flow));
        }

        [Test]
        public void GetStudentsInGroupWithoutOgnp()
        {
            var megaFaculty2 = new MegaFaculty("Not ITIP");
            _ognpManager.AddMegaFaculty(megaFaculty2);
            var megaFaculty3 = new MegaFaculty("Another not ITIP");
            _ognpManager.AddMegaFaculty(megaFaculty3);
            Group group = _ognpManager.AddGroup("M3206", megaFaculty2);
            Ognp ognp2 = _ognpManager.AddOgnp(megaFaculty3, "Some discipline");
            Flow flow = ognp.AddFlow("B12");
            Flow flow2 = ognp2.AddFlow("A12");
            group.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,10,0,0),
                new DateTime(2021,9,1,11,30,0),325,"Stepan Stepanov" ));
            flow.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,13,30,0),
                new DateTime(2021,9,1,15,0,0),325,"Stepan Stepanov" ));
            flow2.AddLessonToSchedule(new Lesson(new DateTime(2021,9,1,8,20,0),
                new DateTime(2021,9,1,9,50,0),325,"Stepan Stepanov" ));
            Student student1 = _ognpManager.AddStudent(group, "Ivan Ivanov");
            _ognpManager.AddStudentToOgnp(ognp, student1, flow);
            _ognpManager.AddStudentToOgnp(ognp2, student1, flow2);
            Student student2 = _ognpManager.AddStudent(group, "Petr Petrov");
            Student student3 = _ognpManager.AddStudent(group, "Matvey Matveev");
            var students = new List<Student>() {student2, student3};
            Assert.AreEqual(students, _ognpManager.GetStudentsWithNoOgnp(group));
        }
        
    }
}