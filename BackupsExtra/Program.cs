using System;
using System.Collections.Generic;
using Backups;
using Backups.Configs;
using Backups.FileSystem;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            List<string> jobObjectFiles = new List<string>()
            {
                @"C:\Users\DELL\Documents\logs\ultra1",
                @"C:\Users\DELL\Documents\logs\ultra2",
            };
            JobObject jobObject = new JobObject(jobObjectFiles);
            BackupJob backupJob = new BackupJob(jobObject, new Logger(LogType.Console, false));
            IRepository repository = new FileSystemRepository(jobObjectFiles);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\logs\Test", repository, DateTime.Now);
            Serialization.SaveViaDataContractSerialization(backupJob, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test\data.xml");
            backupJob = null;
            backupJob = Serialization.LoadViaDataContractSerialization<BackupJob>(
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test\data.xml");
            Console.WriteLine(backupJob.RestorePoints.Count);
        }
    }
}