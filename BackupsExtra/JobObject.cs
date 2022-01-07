using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BackupsExtra
{
    [DataContract]
    public class JobObject
    {
        public JobObject(List<string> jobsFiles)
        {
            Files = jobsFiles;
        }

        private JobObject() { } // required for serialization
        [DataMember]
        public List<string> Files { get; internal set; }

        public void RemoveFileFromJob(string file)
        {
            Files.Remove(file);
        }
    }
}