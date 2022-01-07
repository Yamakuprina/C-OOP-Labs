using System.Collections.Generic;

namespace BackupsExtra
{
    public interface ILimit
    {
        public void CheckBelowLimit(List<RestorePoint> restorePoints);
        public List<RestorePoint> GetNotNeededRestorePoints();
    }
}