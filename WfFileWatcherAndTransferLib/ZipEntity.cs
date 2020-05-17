using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string BackupFileName { get; set; }
        public DateTime ZipCompressionStarted { get; set; }
        public DateTime ZipCompressionFinished { get; set; }
        public TimeSpan ZipCompressionDuration { get; set; }
        public string ZipFileName { get; set; }
        public DateTime CopyZipStarted;
        public DateTime CopyZipFinished;
        public TimeSpan CopyZipDuration;
    }
}
