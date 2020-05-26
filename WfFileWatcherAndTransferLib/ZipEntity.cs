using System;

namespace WfFileWatcherAndTransferLib
{
    /// <summary>
    /// The Entity the that goes process of ..
    ///     first Getting zipped
    ///     Then transferred to a remote file share
    /// </summary>
    public class ZipEntity
    {
        public ZipEntity()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }

        /// <summary>
        /// Full file name including the path to the file
        /// </summary>
        public string BackupFileName { get; set; }

        /// <summary>
        /// Full file name including the path to the file
        /// </summary>
        public DateTime ZipCompressionStarted { get; set; }

        public string RemoteOutputPath { get; set; }
        public string RemoteOutputFileName { get; set; }

        public DateTime ZipCompressionFinished { get; set; }
        public TimeSpan ZipCompressionDuration { get; set; }
        /// <summary>
        /// Full file name including the path to the file
        /// </summary>
        public string ZipFileName { get; set; }
        public DateTime CopyZipStarted;
        public DateTime CopyZipFinished;
        public TimeSpan CopyZipDuration;
    }
}