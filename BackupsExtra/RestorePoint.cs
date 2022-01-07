using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backups;
using Backups.Configs;

namespace BackupsExtra
{
    [DataContract]
    public class RestorePoint
    {
        public RestorePoint(JobObject jobObject, List<string> storagesPaths, DateTime dateTime, int numberOfRestorePoint, FileSystemConf fileSystemConf)
        {
            Storages = new List<string>();
            CreationTime = dateTime;
            Object = jobObject;
            Storages.AddRange(storagesPaths);
            NumberOfRestorePoint = numberOfRestorePoint;
            FileSystemConf = fileSystemConf;
        }

        private RestorePoint() { } // required for serialization
        [DataMember]
        public int NumberOfRestorePoint { get; internal set; }
        [DataMember]
        public JobObject Object { get; internal set; }
        [DataMember]
        public List<string> Storages { get; internal set; }
        [DataMember]
        public DateTime CreationTime { get; internal set; }
        [DataMember]
        public FileSystemConf FileSystemConf { get; internal set; }
        [DataMember]
        public string RestoredPath { get; set; }
    }
}