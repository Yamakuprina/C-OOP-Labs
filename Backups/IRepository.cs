using System.Collections.Generic;
using Backups.Configs;

namespace Backups
{
    public interface IRepository
    {
        List<string> GetStorages();

        List<string> Save(StorageTypeConf storageTypeConf, FileSystemConf fileSystemConf, int numberOfRestorePoint, string pathToSave = "");
    }
}