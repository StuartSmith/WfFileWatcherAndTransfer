using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfFileWatcherAndTransferLib.Logging
{
    public interface ILogWriter
    {
       void LogMessage(string logMessage);
        void LogErrorMessage(string logMessage);
    }
}
