using System.ComponentModel;
using System.Diagnostics;

namespace WfFileWatcherAndTransferLib.Logging
{
    public class EventLogWriter : ILogWriter
    {
        public static EventLogWriter Instance { get; } = new EventLogWriter();

        // ReSharper disable once InconsistentNaming
        private string sourceName = "WfFileWatcherAndTransfer";

        private EventLog WfFileWatcherLog;

        static EventLogWriter()
        {
        }

        private EventLogWriter()
        {
            WfFileWatcherLog = new System.Diagnostics.EventLog
            {
                Source = sourceName,
                Log = sourceName
            };

            //Initialize the event log
            ((ISupportInitialize)(WfFileWatcherLog)).BeginInit();
            if (!EventLog.SourceExists(WfFileWatcherLog.Source))
            {
                EventLog.CreateEventSource(WfFileWatcherLog.Source, WfFileWatcherLog.Log);
            }
            ((ISupportInitialize)(WfFileWatcherLog)).EndInit();
        }

        public void LogMessage(string message)
        {
            WfFileWatcherLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void LogErrorMessage(string Message)
        {
            WfFileWatcherLog.WriteEntry(Message, EventLogEntryType.Error);
        }
    }
}