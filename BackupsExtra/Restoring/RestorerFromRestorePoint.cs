using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups;
using Backups.Configs;

namespace BackupsExtra
{
    public class RestorerFromRestorePoint
    {
        public static void RestoreFromRestorePoint(RestorePoint restorePoint, LocationToRestore locationToRestore, string differentLocation = default)
        {
            switch (locationToRestore)
            {
                case LocationToRestore.OriginalLocation:
                    string originalLocation = restorePoint.Object.Files[0];
                    originalLocation = originalLocation[.. (originalLocation.LastIndexOf(Path.DirectorySeparatorChar.ToString()) - 1)];
                    string pathToRestDirectory = originalLocation + @"\" + "Restored" + restorePoint.NumberOfRestorePoint;
                    switch (restorePoint.FileSystemConf)
                    {
                        case FileSystemConf.Folder:
                            Directory.CreateDirectory(pathToRestDirectory);
                            foreach (string storage in restorePoint.Storages)
                            {
                                ZipFile.ExtractToDirectory(storage, pathToRestDirectory);
                            }

                            restorePoint.RestoredPath = pathToRestDirectory;
                            break;
                        case FileSystemConf.Tests:
                            restorePoint.RestoredPath = pathToRestDirectory;
                            break;
                    }

                    break;
                case LocationToRestore.DifferentLocation:
                    if (differentLocation == default) throw new BackupException("No location found");
                    string pathToDirectory = differentLocation + @"\" + "Restored" + restorePoint.NumberOfRestorePoint;
                    switch (restorePoint.FileSystemConf)
                    {
                        case FileSystemConf.Folder:
                            Directory.CreateDirectory(pathToDirectory);
                            foreach (string storage in restorePoint.Storages)
                            {
                                ZipFile.ExtractToDirectory(storage, pathToDirectory);
                            }

                            restorePoint.RestoredPath = pathToDirectory;
                            break;
                        case FileSystemConf.Tests:
                            restorePoint.RestoredPath = pathToDirectory;
                            break;
                    }

                    break;
            }
        }
    }
}