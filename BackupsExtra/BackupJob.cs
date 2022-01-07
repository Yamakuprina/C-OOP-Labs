using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Backups;
using Backups.Configs;

namespace BackupsExtra
{
    [DataContract]
    public class BackupJob
    {
        [DataMember]
        private List<RestorePoint> restorePoints;
        public BackupJob(JobObject job, Logger logger)
        {
            restorePoints = new List<RestorePoint>();
            JobObject = job;
            NumberOfRestorePoint = 0;
            Logger = logger;
            RestorePoints = restorePoints.AsReadOnly();
        }

        private BackupJob() { } // required for serialization
        public IReadOnlyList<RestorePoint> RestorePoints { get; internal set; }
        [DataMember]
        public int NumberOfRestorePoint { get; internal set; }
        [DataMember]
        public JobObject JobObject { get; internal set; }

        [DataMember]
        private Logger Logger { get; set; }

        public void RemoveJobObject(string job)
        {
            JobObject.RemoveFileFromJob(job);
        }

        public void AddNewRestorePoint(StorageTypeConf storageTypeConf, FileSystemConf fileSystemConf, string outputDirectory, IRepository repository, DateTime dateTime)
        {
            List<string> newStorages =
                repository.Save(storageTypeConf, fileSystemConf, NumberOfRestorePoint, outputDirectory);
            var restorePoint = new RestorePoint(JobObject, newStorages, dateTime, NumberOfRestorePoint, fileSystemConf);
            restorePoints.Add(restorePoint);
            NumberOfRestorePoint++;
            Logger.Info($"Restore point {NumberOfRestorePoint} created");
        }

        public void MergeRestorePoints(List<RestorePoint> restorePointsNotNeeded)
        {
            restorePoints.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
            for (int i = 0; i < restorePoints.Count; i++)
            {
                if (!restorePointsNotNeeded.Contains(restorePoints[i])) continue;

                RestorePoint restorePointNotNeeded = restorePoints[i];
                if (i + 1 == restorePoints.Count)
                    throw new BackupException("No restore points to merge with");
                RestorePoint restorePointToMergeWith = restorePoints[i + 1];

                if (restorePointNotNeeded.Storages.Count == 1)
                {
                    restorePoints.Remove(restorePointNotNeeded);
                    return;
                }

                foreach (string storage in restorePointNotNeeded.Storages)
                {
                    string fileNameWithDate =
                        storage[
                            (storage.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) +
                             1) ..];
                    string fileName = fileNameWithDate[.. fileNameWithDate.LastIndexOf("_")];
                    if (!restorePointToMergeWith.Storages.Exists(s => s.Contains(fileName)))
                    {
                        string pathToNewFileLocation = restorePointToMergeWith.Storages[0];
                        pathToNewFileLocation =
                            pathToNewFileLocation[
                                .. pathToNewFileLocation.LastIndexOf(Path.DirectorySeparatorChar.ToString())];
                        pathToNewFileLocation = pathToNewFileLocation + Path.DirectorySeparatorChar.ToString() +
                                                fileNameWithDate;
                        File.Copy(storage, pathToNewFileLocation);
                        restorePointToMergeWith.Storages.Add(pathToNewFileLocation);
                    }
                }

                restorePoints.Remove(restorePointNotNeeded);
            }
        }

        public void DeleteRestorePoints(List<RestorePoint> restorePointsToDelete)
        {
            foreach (RestorePoint restorePointToDelete in restorePointsToDelete)
            {
                restorePoints.Remove(restorePointToDelete);
            }
        }
    }
}