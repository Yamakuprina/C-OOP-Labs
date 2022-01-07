using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Configs;

namespace Backups
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints;
        private int numberOfRestorePoint;
        private JobObject _jobObject;

        public BackupJob(JobObject job)
        {
            _restorePoints = new List<RestorePoint>();
            _jobObject = job;
            numberOfRestorePoint = 0;
            RestorePoints = _restorePoints.AsReadOnly();
        }

        public IReadOnlyList<RestorePoint> RestorePoints { get; }

        public void RemoveJobObject(string job)
        {
            _jobObject.RemoveFileFromJob(job);
        }

        public List<string> GetListOfStorages()
        {
            return _restorePoints.SelectMany(restorePoint => restorePoint.Storages).ToList();
        }

        public void AddNewRestorePoint(StorageTypeConf storageTypeConf, FileSystemConf fileSystemConf, string outputDirectory, IRepository repository, DateTime dateTime)
        {
            List<string> newStorages = repository.Save(storageTypeConf, fileSystemConf, numberOfRestorePoint, outputDirectory);
            var restorePoint = new RestorePoint(_jobObject, newStorages, dateTime);
            _restorePoints.Add(restorePoint);
            numberOfRestorePoint++;
        }
    }
}