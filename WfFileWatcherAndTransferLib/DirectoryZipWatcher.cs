using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WfFileWatcherAndTransferLib.Logging;

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
        private ZipWatcherPreferences _zipWatcherPreferences;
        private bool _isSevenZipRunning;

        public void StartWatch(ZipWatcherPreferences zipWatcherPreferences)
        {
            _zipWatcherPreferences = zipWatcherPreferences;

            if (!Directory.Exists(_zipWatcherPreferences.PathToWatch))
            {
                throw new Exception($"Folder does not exist: {_zipWatcherPreferences.PathToWatch}. Please create folder to watch");
            }

            _fSWatcher = new FileSystemWatcher();

            _fSWatcher.Path = zipWatcherPreferences.PathToWatch;
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

            AllLogWriter.Instance.LogMessage($"Starting to Watch Folder {zipWatcherPreferences.PathToWatch}");

            _fSWatcher.Created += FSWatcher_Created;

            AllLogWriter.Instance.LogMessage($"Watching Folder {zipWatcherPreferences.PathToWatch}");
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            AllLogWriter.Instance.LogMessage($"\nAdding File to Queue for zip processing: {e.FullPath}");
            _zipItemQueue.Add(new ZipEntity() { BackupFileName = e.FullPath });
        }

        private async Task<bool> ZippingFiles()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                while (true)
                {
                    var zipEntity = _zipItemQueue.Take();
                    zipEntity.ZipCompressionStarted = DateTime.Now;
                    UpdateZipFileNameAndPath(zipEntity);
                    AllLogWriter.Instance.LogMessage($"\nZipping file {zipEntity.BackupFileName} to {zipEntity.ZipFileName} in folder {_zipWatcherPreferences.OutPutFolderFor7zip}");
                    Call7zip(_zipWatcherPreferences, zipEntity);

                    //Finished Compressing file
                    zipEntity.CopyZipFinished = DateTime.Now;
                    zipEntity.ZipCompressionDuration = zipEntity.CopyZipFinished - zipEntity.ZipCompressionStarted;

                    var ts = zipEntity.ZipCompressionDuration;
                    string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
                    AllLogWriter.Instance.LogMessage($"\nFinished zipping file for {zipEntity.BackupFileName} to {zipEntity.ZipFileName} duration:{elapsedTime}");
                }
            }
            catch (Exception e)
            {
                AllLogWriter.Instance.LogErrorMessage(e.ToString());
                throw;
            }
        }

        private void UpdateZipFileNameAndPath(ZipEntity zEntity)
        {
            var filename = Path.GetFileNameWithoutExtension(zEntity.BackupFileName);
            filename = filename.Replace(" ","");
            filename = filename.Replace("(", "");
            filename = filename.Replace(")", "");
            filename = filename.Replace("-", "");

            zEntity.ZipFileName = $"{Path.Combine(_zipWatcherPreferences.OutPutFolderFor7zip, filename)}.7z";
        }

        private void Call7zip(ZipWatcherPreferences zipWatcherPreferences, ZipEntity zipEntity)
        {
            Process sevenZip = new Process();

            if (!(string.IsNullOrEmpty(zipWatcherPreferences.PathTo7Zip)))
            {
                sevenZip.StartInfo.WorkingDirectory = zipWatcherPreferences.PathTo7Zip;
            }



            sevenZip.StartInfo.UseShellExecute = false;
            sevenZip.StartInfo.RedirectStandardOutput = true;

            sevenZip.StartInfo.Arguments = $"a -m0=lzma2 -r -y {zipEntity.ZipFileName} \"{zipEntity.BackupFileName}\" ";
            sevenZip.StartInfo.FileName = "7z.exe";
            try
            {
                FileLogWriter.Instance.LogMessage("Starting 7zip");
                FileLogWriter.Instance.LogMessage($"7zip Commandline {Path.Combine(sevenZip.StartInfo.WorkingDirectory,"7z.exe")} {sevenZip.StartInfo.Arguments} ");


                sevenZip.Start();
                _isSevenZipRunning = true;

                
                while (!(sevenZip.StandardOutput.EndOfStream) && _isSevenZipRunning)
                    FileLogWriter.Instance.LogMessage($"\t{sevenZip.StandardOutput.ReadLine()}");

                if (_isSevenZipRunning == true)
                    sevenZip.WaitForExit();

                FileLogWriter.Instance.LogMessage($"Finished Compressing {zipEntity.ZipFileName}");
                _isSevenZipRunning = false;

            }
            catch (Exception e)
            {
                AllLogWriter.Instance.LogErrorMessage(e.ToString());
            }
        }
    }
}