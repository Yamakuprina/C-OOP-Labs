using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Configs;

namespace Backups.FileSystem
{
    public class FileSystemSplit
    {
        private List<string> _filesToArchivatePaths;
        private List<string> _archivatedFiles;

        public FileSystemSplit(List<string> filesToArchivatePaths)
        {
            _filesToArchivatePaths = filesToArchivatePaths;
            _archivatedFiles = new List<string>();
        }

        public List<string> GetStorages()
        {
            return _archivatedFiles;
        }

        public void Archivate(FileSystemConf fileSystemConf, int numberOfRestorePoint, string outputDirectoryPath)
        {
            switch (fileSystemConf)
            {
                case FileSystemConf.Folder:
                    string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
                    Directory.CreateDirectory(pathToStore);
                    int fileNumber = 1;
                    foreach (string filePath in _filesToArchivatePaths)
                    {
                        string fileName = filePath[(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
                        string dateTime = DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
                        string pathToTempFolder = pathToStore + "_" + fileName + fileNumber + "_" + dateTime;
                        Directory.CreateDirectory(pathToTempFolder);
                        File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
                        ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
                        File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTime.Now + ".zip");
                        _archivatedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTime.Now + ".zip");
                        Directory.Delete(pathToTempFolder, true);
                        fileNumber++;
                    }

                    return;
                case FileSystemConf.Tests:
                    pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
                    fileNumber = 1;
                    foreach (string filePath in _filesToArchivatePaths)
                    {
                        string fileName = filePath[(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
                        _archivatedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTime.Now + ".zip");
                        fileNumber++;
                    }

                    return;
            }
        }
    }
}