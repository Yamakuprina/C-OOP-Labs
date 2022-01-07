using System;
using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class ClearingRestorePointsSettings
    {
        public ClearingRestorePointsSettings(List<ILimit> limits, ExterminationType userExterminationType, HybridType hybridType = default)
        {
            Limits = limits;
            UserExterminationType = userExterminationType;
            UserHybridType = hybridType;
        }

        public List<ILimit> Limits { get; set; }
        public ExterminationType UserExterminationType { get; set; }
        public HybridType UserHybridType { get; set; }

        public bool IsHybrid()
        {
            if (Limits.Count > 1)
            {
                if (UserHybridType == default)
                {
                    throw new BackupException("ClearingRestorePointsSettings is HybridLimit, but HybridType not set.");
                }

                return true;
            }

            return false;
        }
    }
}