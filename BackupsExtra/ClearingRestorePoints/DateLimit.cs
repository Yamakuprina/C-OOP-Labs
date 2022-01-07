using System;
using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class DateLimit : ILimit
    {
        private DateTime dateLimit;
        private List<RestorePoint> notNeededRestorePoints;

        public DateLimit(DateTime dateLimit)
        {
            if (dateLimit == null)
            {
                throw new BackupException("Wrong args for DateLimit");
            }

            this.dateLimit = dateLimit;
            notNeededRestorePoints = new List<RestorePoint>();
        }

        public void CheckBelowLimit(List<RestorePoint> restorePoints)
        {
            foreach (RestorePoint restorePoint in restorePoints)
            {
                if (restorePoint.CreationTime > dateLimit) continue;
                notNeededRestorePoints.Add(restorePoint);
            }

            if (notNeededRestorePoints.Count == restorePoints.Count)
            {
                throw new BackupException("Cant delete all Restore points.");
            }
        }

        public List<RestorePoint> GetNotNeededRestorePoints()
        {
            return notNeededRestorePoints;
        }
    }
}