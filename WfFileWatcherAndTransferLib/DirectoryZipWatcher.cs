using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

// ReSharper disable RedundantCast
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable UseObjectOrCollectionInitializer

namespace WfFileWatcherAndTransferLib
{
    public class DirectoryZipWatcher
    {
        private FileSystemWatcher _fSWatcher;
        private BlockingCollection<ZipEntity> _zipItemQueue = new BlockingCollection<ZipEntity>();
        private Task<bool> _zipFileTask;
        private ZipWatcherPreferences _zipWatcherInit;

        public void StartWatch(ZipWatcherPreferences zipWatcherInit)
        {
            _zipWatcherInit = zipWatcherInit;

            if (!Directory.Exists(zipWatcherInit.PathToWatch))
            {
                throw new Exception($"Folder does not exist: {zipWatcherInit.PathToWatch}. Please create folder to watch");
            }

            _fSWatcher = new FileSystemWatcher();

            _fSWatcher.Path = zipWatcherInit.PathToWatch;
            _fSWatcher.NotifyFilter =
                ((NotifyFilters)((((((((NotifyFilters.FileName |
                                                   NotifyFilters.DirectoryName)
                                                  | NotifyFilters.Attributes)
                                                 | NotifyFilters.Size)
                                                | NotifyFilters.LastWrite)
                                               | NotifyFilters.LastAccess)
                                              | NotifyFilters.CreationTime)
                                             | NotifyFilters.Security)));

            _fSWatcher.EnableRaisingEvents = true;
            _fSWatcher.IncludeSubdirectories = false;

            _zipFileTask = ZippingFiles();

            WfFileWatcherEventLog.Instance.LogMessage($"Starting to Watch Folder {zipWatcherInit.PathToWatch}");

            _fSWatcher.Created += FSWatcher_Created;

            WfFileWatcherEventLog.Instance.LogMessage($"Watching Folder {zipWatcherInit.PathToWatch}");
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            WfFileWatcherEventLog.Instance.LogMessage($"\nAdding File to Queue for zip processing {e.FullPath}");
            _zipItemQueue.Add(new ZipEntity() { BackupFileName = e.FullPath });
        }

        private async Task<bool> ZippingFiles()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                while (true)
                {
                    var info = _zipItemQueue.Take();
                    WfFileWatcherEventLog.Instance.LogMessage($"\nZipping file {info.ZipFileName} to {_zipWatcherInit.OutPutFolderFor7zip}");
                }
            }
            catch (Exception e)
            {
                WfFileWatcherEventLog.Instance.LogErrorMessage(e.ToString());
                throw;
            }
        }
    }
}