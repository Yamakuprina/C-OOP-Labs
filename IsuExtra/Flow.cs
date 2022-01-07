namespace IsuExtra
{
    public class Flow : Group
    {
        public Flow(string name, MegaFaculty megaFaculty, Ognp ognp, int maxStudent = 32)
            : base(name, megaFaculty, maxStudent = 32)
        {
            Name = name;
            CourseNumber = (CourseNumber)int.Parse(string.Empty + name[1]);
            MaxStudent = maxStudent;
            MegaFaculty = megaFaculty;
            Ognp = ognp;
        }

        public Ognp Ognp { get; set; }
    }
}