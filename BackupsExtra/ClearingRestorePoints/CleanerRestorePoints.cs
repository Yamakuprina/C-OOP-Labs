using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupsExtra
{
    public class CleanerRestorePoints
    {
        private List<RestorePoint> RestorePointsTrash { get; set; } = new List<RestorePoint>();
        public void ClearRestorePointsSettings(BackupJob backupJob, ClearingRestorePointsSettings clearingRestorePointsSettings)
        {
            if (clearingRestorePointsSettings.IsHybrid())
            {
                switch (clearingRestorePointsSettings.UserHybridType)
                {
                    case HybridType.HybridAny:
                        foreach (ILimit limit in clearingRestorePointsSettings.Limits)
                        {
                            limit.CheckBelowLimit(backupJob.RestorePoints.ToList());
                            AddRestorePointsToTrash(limit.GetNotNeededRestorePoints());
                        }

                        break;
                    case HybridType.HybridAll:
                        clearingRestorePointsSettings.Limits.ForEach(limit => limit.CheckBelowLimit(backupJob.RestorePoints.ToList()));
                        foreach (RestorePoint restorePoint in backupJob.RestorePoints)
                        {
                            if (!clearingRestorePointsSettings.Limits.All(limit => limit.GetNotNeededRestorePoints().Contains(restorePoint))) break;
                            AddRestorePointToTrash(restorePoint);
                        }

                        break;
                }

                switch (clearingRestorePointsSettings.UserExterminationType)
                {
                    case ExterminationType.Delete:
                        backupJob.DeleteRestorePoints(RestorePointsTrash);
                        break;
                    case ExterminationType.Merge:
                        backupJob.MergeRestorePoints(RestorePointsTrash);
                        break;
                }
            }
            else
            {
                clearingRestorePointsSettings.Limits.First().CheckBelowLimit(backupJob.RestorePoints.ToList());
                AddRestorePointsToTrash(clearingRestorePointsSettings.Limits.First().GetNotNeededRestorePoints());
                switch (clearingRestorePointsSettings.UserExterminationType)
                {
                    case ExterminationType.Delete:
                        backupJob.DeleteRestorePoints(RestorePointsTrash);
                        break;
                    case ExterminationType.Merge:
                        backupJob.MergeRestorePoints(RestorePointsTrash);
                        break;
                }
            }
        }

        private void AddRestorePointsToTrash(List<RestorePoint> restorePoints)
        {
            RestorePointsTrash.AddRange(restorePoints);
            RestorePointsTrash = RestorePointsTrash.Distinct().ToList();
        }

        private void AddRestorePointToTrash(RestorePoint restorePoint)
        {
            RestorePointsTrash.Add(restorePoint);
            RestorePointsTrash = RestorePointsTrash.Distinct().ToList();
        }
    }
}