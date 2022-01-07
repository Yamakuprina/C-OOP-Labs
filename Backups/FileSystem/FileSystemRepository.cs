using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Configs;

namespace Backups.FileSystem
{
    public class FileSystemRepository : IRepository
    {
        private readonly List<string> _listOfStorages = new List<string>();
        private List<string> _listOfJobObjects;

        public FileSystemRepository(List<string> listOfObjects)
        {
            _listOfJobObjects = listOfObjects;
        }

        public List<string> GetStorages()
        {
            return _listOfStorages;
        }

        public List<string> Save(StorageTypeConf storageTypeConf, FileSystemConf fileSystemConf, int numberOfRestorePoint, string pathToSave)
        {
            switch (storageTypeConf)
            {
                case StorageTypeConf.Single:
                    var fileSystemSingle = new FileSystemSingle(_listOfJobObjects);
                    fileSystemSingle.Archivate(fileSystemConf, numberOfRestorePoint, pathToSave);
                    _listOfStorages.AddRange(fileSystemSingle.GetStorages());
                    return fileSystemSingle.GetStorages();
                case StorageTypeConf.Split:
                    var fileSystemSplit = new FileSystemSplit(_listOfJobObjects);
                    fileSystemSplit.Archivate(fileSystemConf, numberOfRestorePoint, pathToSave);
                    _listOfStorages.AddRange(fileSystemSplit.GetStorages());
                    return fileSystemSplit.GetStorages();
                default:
                    throw new BackupException("Storage Type wasnt chosen!");
            }
        }
    }
}