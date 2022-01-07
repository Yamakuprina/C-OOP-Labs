using System;
using System.Collections.Generic;

namespace Backups
{
    public class RestorePoint
    {
        private JobObject _object;
        private List<string> storages;

        public RestorePoint(JobObject jobObject, List<string> storagesPaths, DateTime dateTime)
        {
            storages = new List<string>();
            CreationTime = dateTime;
            _object = jobObject;
            storages.AddRange(storagesPaths);
            Storages = storages.AsReadOnly();
        }

        public DateTime CreationTime { get; }
        public IReadOnlyList<string> Storages { get; }
    }
}