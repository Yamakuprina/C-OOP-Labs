using System.Collections.Generic;

namespace IsuExtra
{
    public class Ognp
    {
        private List<Flow> allFlows = new List<Flow>();

        public Ognp(MegaFaculty megaFaculty, string name)
        {
            MegaFaculty = megaFaculty;
            Name = name;
        }

        public MegaFaculty MegaFaculty { get; }
        public IReadOnlyList<Flow> AllFlows => allFlows.AsReadOnly();

        public string Name { get; set; }

        public Flow AddFlow(string name)
        {
            var newFlow = new Flow(name, MegaFaculty, this);
            allFlows.Add(newFlow);
            return newFlow;
        }
    }
}