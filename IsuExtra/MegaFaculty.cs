using System.Collections.Generic;

namespace IsuExtra
{
    public class MegaFaculty
    {
        private List<Group> allGroups = new List<Group>();

        public MegaFaculty(string name)
        {
            Name = name;
        }

        public Ognp Ognp { get; set; }
        public IReadOnlyList<Group> AllGroups => allGroups.AsReadOnly();

        public string Name { get; set; }

        public void AddGroup(Group @group)
        {
            allGroups.Add(@group);
        }
    }
}