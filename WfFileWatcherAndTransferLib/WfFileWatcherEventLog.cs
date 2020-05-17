using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace WfFileWatcherAndTransferLib
{
    public sealed class WfFileWatcherEventLog
    {
        public static WfFileWatcherEventLog Instance { get; } = new WfFileWatcherEventLog();

        // ReSharper disable once InconsistentNaming
        private string sourceName = "WfFileWatcherAndTransfer";

        private EventLog WfFileWatcherLog;

        static WfFileWatcherEventLog()
        {
        }

        private WfFileWatcherEventLog()
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
            Console.WriteLine(message);
            WfFileWatcherLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void LogErrorMessage(string Message)
        {
            Console.WriteLine("Error Occured:/n");
            Console.WriteLine(Message);
            WfFileWatcherLog.WriteEntry(Message, EventLogEntryType.Information);
        }
    }
}
