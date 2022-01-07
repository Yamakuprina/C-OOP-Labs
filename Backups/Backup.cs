using System.Collections.Generic;

namespace Backups
{
    public class Backup
    {
        private BackupJob _backupJob;

        public BackupJob CreateBackupJob(JobObject jobObject)
        {
            if (jobObject == null)
            {
                throw new BackupException("No jobObject found!");
            }

            _backupJob = new BackupJob(jobObject);
            return _backupJob;
        }
    }
}