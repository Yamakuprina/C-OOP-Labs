using System.Runtime.Serialization;
using Backups;

namespace BackupsExtra
{
    [DataContract]
    public class Backup
    {
        [DataMember]
        private BackupsExtra.BackupJob _backupJob;

        public BackupsExtra.BackupJob CreateBackupJob(BackupsExtra.JobObject jobObject, Logger logger)
        {
            if (jobObject == null)
            {
                throw new BackupException("No jobObject found!");
            }

            _backupJob = new BackupsExtra.BackupJob(jobObject, logger);
            return _backupJob;
        }
    }
}