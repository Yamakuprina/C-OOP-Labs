using System;
using System.IO;
using System.Runtime.Serialization;
using Backups;
using NUnit.Framework.Internal;

namespace BackupsExtra
{
    [DataContract]
    public class Logger : ILogger
    {
        public Logger(LogType userLogType, bool requiredTimeCode, string logFile = default)
        {
            UserLogType = userLogType;
            RequiredTimeCode = requiredTimeCode;
            LogFile = logFile;
        }

        private Logger() { } // required for serialization

        [DataMember]
        public LogType UserLogType { get; internal set; }
        [DataMember]
        public bool RequiredTimeCode { get; internal set; }
        [DataMember]
        public string LogFile { get; internal set; }

        public void Error(string message)
        {
            Write("Error:");
            Write(message);
        }

        public void Error(string message, params object[] args)
        {
            Write("Error:");
            Write(message);
        }

        public void Warning(string message)
        {
            Write("Warning:");
            Write(message);
        }

        public void Warning(string message, params object[] args)
        {
            Write("Warning:");
            Write(message);
        }

        public void Info(string message)
        {
            Write("Info:");
            Write(message);
        }

        public void Info(string message, params object[] args)
        {
            Write("Info:");
            Write(message);
        }

        public void Debug(string message)
        {
            Write("Debug:");
            Write(message);
        }

        public void Debug(string message, params object[] args)
        {
            Write("Debug:");
            Write(message);
        }

        private void Write(string message)
        {
            string dateTime = DateTime.Now.ToString("MM dd yy");
            switch (UserLogType)
            {
                case LogType.Console:
                    if (RequiredTimeCode) Console.WriteLine(dateTime + "_" + message);
                    else Console.WriteLine(message);
                    break;
                case LogType.LogFile:
                    if (LogFile == default) throw new BackupException("LogFile is not initialized");
                    var logfile = new StreamWriter(LogFile);
                    if (RequiredTimeCode) logfile.WriteLine(dateTime + "_" + message);
                    else logfile.WriteLine(message);
                    logfile.Close();
                    break;
            }
        }
    }
}