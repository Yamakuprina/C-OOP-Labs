using System;
using System.Collections.Generic;
using Backups.Configs;
using Backups.FileSystem;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupTest1
    {
        private Backup _backup;

        [SetUp]
        public void Setup()
        {
            _backup = new Backup();
        }

        [Test]
        public void AddBackupToBackupJob()
        {
            var list = new List<string>()
            {
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Backup.cs",
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\BackupJob.cs",
            };
            BackupJob backupJob = _backup.CreateBackupJob(new JobObject(list));
            IRepository repository = new FileSystemRepository(list);
            backupJob.AddNewRestorePoint(StorageTypeConf.Split, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Test", repository, DateTime.Now);
            backupJob.RemoveJobObject(@"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Backup.cs");
            backupJob.AddNewRestorePoint(StorageTypeConf.Split, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Test", repository, DateTime.Now);
            Assert.AreEqual(backupJob.GetListOfStorages().Count, 3);
            Assert.AreEqual(backupJob.RestorePoints.Count, 2);
        }
    }
}