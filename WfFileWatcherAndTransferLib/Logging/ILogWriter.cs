namespace WfFileWatcherAndTransferLib.Logging
{
    public interface ILogWriter
    {
        void LogMessage(string logMessage);

        void LogErrorMessage(string logMessage);
    }
}