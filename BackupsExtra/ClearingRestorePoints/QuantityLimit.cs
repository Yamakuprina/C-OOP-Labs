using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class QuantityLimit : ILimit
    {
        private int quantityLimit;
        private List<RestorePoint> notNeededRestorePoints;

        public QuantityLimit(int quantityLimit)
        {
            if (quantityLimit <= 0)
            {
                throw new BackupException("Cant delete all Restore points.");
            }

            this.quantityLimit = quantityLimit;
            notNeededRestorePoints = new List<RestorePoint>();
        }

        public void CheckBelowLimit(List<RestorePoint> restorePoints)
        {
            restorePoints.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
            int quantityToDelete = restorePoints.Count - quantityLimit;
            for (int i = 0; i < restorePoints.Count; i++)
            {
                if (quantityToDelete <= 0) continue;
                notNeededRestorePoints.Add(restorePoints[i]);
                quantityToDelete--;
            }
        }

        public List<RestorePoint> GetNotNeededRestorePoints()
        {
            return notNeededRestorePoints;
        }
    }
}