using System;
using System.Collections.Generic;
using Backups.Configs;
using Backups.FileSystem;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var list = new List<string>()
            {
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Backup.cs",
                @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\BackupJob.cs",
            };
            var backup = new Backup();
            BackupJob backupJob = backup.CreateBackupJob(new JobObject(list));
            IRepository repository = new FileSystemRepository(list);
            backupJob.AddNewRestorePoint(StorageTypeConf.Single, FileSystemConf.Folder, @"C:\Users\DELL\Documents\GitHub\Yamakuprina\Backups\Test", repository, DateTime.Now);
        }
    }
}
