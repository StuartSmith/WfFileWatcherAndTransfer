using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace WfFileWatcherAndTransferLib.Logging
{
    public class FileLogWriter : ILogWriter
    {
        public static FileLogWriter Instance { get; } = new FileLogWriter();

        private BlockingCollection<string> _LogLineQueue = new BlockingCollection<string>();

        private Task<bool> _WritingLogTask;

        private FileLogWriter()
        {
            _WritingLogTask = WritingLog();
        }

        public void LogMessage(string logMessage)
        {
            Console.WriteLine(logMessage);
            _LogLineQueue.Add(logMessage);
        }

        public void LogErrorMessage(string logMessage)
        {
            logMessage = "Error Occured:\n{logMessage}";
            Console.WriteLine(logMessage);
            _LogLineQueue.Add(logMessage);
        }

        private async Task<bool> WritingLog()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            while (true)
            {
                var LogLine = _LogLineQueue.Take();

                using (StreamWriter w = File.AppendText(CalculateLogFileName(CreateLogFileFolder())))
                {
                    Log(LogLine, w);
                }
            }
        }

        public void Log(string logMessage, TextWriter w)
        {
            w.Write($"{ DateTime.Now.ToLongTimeString()} {logMessage} \r\n");
        }

        private string CreateLogFileFolder()
        {
            string LogsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(LogsFolder))
                Directory.CreateDirectory(LogsFolder);

            return LogsFolder;
        }

        private string CalculateLogFileName(string LogsFolder)
        {
            DateTime ct = DateTime.Now;

            return Path.Combine(LogsFolder,
                $"TransferLog - { ct.Year:0000}-{ ct.Month:00}-{ ct.Day:00}.log");
        }
    }
}