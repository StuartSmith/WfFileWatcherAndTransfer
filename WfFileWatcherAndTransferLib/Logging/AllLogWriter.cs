namespace WfFileWatcherAndTransferLib.Logging
{
    public class AllLogWriter : ILogWriter
    {
        public static AllLogWriter Instance { get; } = new AllLogWriter();

        private AllLogWriter()
        {
        }

        public void LogMessage(string logMessage)
        {
            EventLogWriter.Instance.LogMessage(logMessage);
            FileLogWriter.Instance.LogMessage(logMessage);
        }

        public void LogErrorMessage(string logMessage)
        {
            EventLogWriter.Instance.LogErrorMessage(logMessage);
            FileLogWriter.Instance.LogErrorMessage(logMessage);
        }
    }
}