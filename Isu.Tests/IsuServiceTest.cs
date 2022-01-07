using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group @group = _isuService.AddGroup("M3206");
            _isuService.AddStudent(@group, "Tagir");
            Student student = _isuService.FindStudent("Tagir");
            Assert.IsNotNull(student.Group);
            Assert.Contains(student, _isuService.FindStudents(@group.Name));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            var @group = new Group("M3206");
            for (int i = 0; i < @group.MaxStudent; i++)
            {
                _isuService.AddStudent(@group, "Ivan");
            }

            Assert.Catch<IsuException>((() =>
            {
                _isuService.AddStudent(@group, "Ivan");
            }));
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M0000");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group1 = _isuService.AddGroup("M3106");
            Group group2 = _isuService.AddGroup("M3206");
            Student student = _isuService.AddStudent(group1, "Tagir");
            _isuService.ChangeStudentGroup(student, group2);
            Assert.That(_isuService.FindStudents(group1.Name), Has.No.Member(student));
            Assert.True((_isuService.FindStudent(student.Name).Name == student.Name) && (_isuService.FindStudent(student.Name).Group == group2));
        }
    }
}