using System;
using System.Collections.Generic;
using System.IO;
using Backups.Configs;
using Backups.FileSystem;
using BackupsExtra;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupTest1
    {
        private BackupsExtra.Backup _backup;
        private List<string> jobObjectFiles;
        private BackupsExtra.JobObject jobObject;
        private BackupsExtra.BackupJob backupJob;
        private IRepository repository;
        private List<ILimit> limits;
        private ClearingRestorePointsSettings settings;

        private CleanerRestorePoints cleaner;

        [SetUp]
        public void Setup()
        {
            _backup = new BackupsExtra.Backup();
            jobObjectFiles = new List<string>()
            {
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Backup.cs",
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\BackupJob.cs",
            };
            jobObject = new BackupsExtra.JobObject(jobObjectFiles);
            backupJob = _backup.CreateBackupJob(jobObject, new Logger(LogType.Console, false));
            repository = new FileSystemRepository(jobObjectFiles);
            limits = new List<ILimit>() {new DateLimit(DateTime.Now)};
            settings = new ClearingRestorePointsSettings(limits, ExterminationType.Delete);
            cleaner = new CleanerRestorePoints();
        }

        [Test]
        public void RestoreFromRestorePoint()
        {
            
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, DateTime.Now);
            RestorerFromRestorePoint.RestoreFromRestorePoint(backupJob.RestorePoints[0], LocationToRestore.DifferentLocation, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test");
            Assert.AreEqual(@"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test\Restored0", backupJob.RestorePoints[0].RestoredPath);
        }

        [Test]
        public void LoggerConsole()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, DateTime.Now);
            Assert.AreEqual("Info:\nRestore point 1 created\n",output.ToString());
        }

        [Test]
        public void ClearingRestorePoints()
        {
            DateTime dateTime = DateTime.Now.AddDays(-1);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, DateTime.Now);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, DateTime.Now);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, dateTime);
            cleaner.ClearRestorePointsSettings(backupJob, settings);
            Assert.AreEqual(2, backupJob.RestorePoints.Count);
        }

        [Test]
        public void SaveAndLoad()
        {
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Tests, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test", repository, DateTime.Now);
            Serialization.SaveViaDataContractSerialization(backupJob, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test\data.xml");
            backupJob = null;
            backupJob = Serialization.LoadViaDataContractSerialization<BackupsExtra.BackupJob>(@"C:\Users\DELL\Documents\GitHub\Yamakuprina\BackupsExtra\Test\data.xml");
            Assert.AreEqual(1, backupJob.NumberOfRestorePoint);
            Assert.AreEqual(@"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Backup.cs", backupJob.JobObject.Files[0]);
        }
    }
}