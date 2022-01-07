using System.Collections.Generic;

namespace Backups
{
    public class JobObject
    {
        private List<string> files;

        public JobObject(List<string> jobsFiles)
        {
            files = jobsFiles;
            Files = files.AsReadOnly();
        }

        public IReadOnlyList<string> Files { get; }

        public void RemoveFileFromJob(string file)
        {
            files.Remove(file);
        }
    }
}