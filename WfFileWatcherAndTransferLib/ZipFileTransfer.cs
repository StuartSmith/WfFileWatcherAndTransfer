using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using WfFileWatcherAndTransferLib.Logging;

namespace WfFileWatcherAndTransferLib
{
    /// <summary>
    /// Object for copying files from local system to a directory share after the
    /// file has been compressed
    /// Using a singleton patern to inforce only one copy of this class for the entire application
    /// </summary>
    public sealed class ZipFileTransfer
    {
        // ReSharper disable once IdentifierTypo
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private BlockingCollection<ZipEntity> _zipTranferQueue = new BlockingCollection<ZipEntity>();

        public static ZipFileTransfer _instance { get; } = new ZipFileTransfer();

        private Task<bool> _zipFileTask;
        private bool _isZipFileTaskRunning = false;

        static ZipFileTransfer()
        {
        }

        private ZipFileTransfer()
        {
        }

        public static ZipFileTransfer Instance => _instance;

        /// <summary>
        /// Add a file to transfer to the transfer Queue
        /// Files will be transferred one at a time
        /// </summary>
        public void AddFileToTransferAsync(ZipEntity zipEntity,ZipWatcherPreferences zipWatcherPreferences)
        {
            
            zipEntity.RemoteOutputPath = CalculateRemotePath(zipEntity, zipWatcherPreferences);
            zipEntity.RemoteOutputFileName=CalculateRemoteFileName(zipEntity, zipWatcherPreferences);
            AllLogWriter.Instance.LogMessage($"\nAdding file to Queue for transferring from: {zipEntity.ZipFileName} to  {zipEntity.RemoteOutputPath}");
            // zipEntity.RemoteOutputFolder=

            

            if (!(_isZipFileTaskRunning))
            {
                _zipFileTask = TransferringFiles();
            }

            _zipTranferQueue.Add(zipEntity);
        }

        /// <summary>
        /// Determines the name of the remote folder to transfer the zip file
        /// </summary>
        /// <param name="zipEntity"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string CalculateRemotePath(ZipEntity zipEntity, ZipWatcherPreferences zipWatcherPreferences)
        {
            
            return Path.Combine(zipWatcherPreferences.EndResultPath, DateTime.Now.GetYearMonthDay());
            
        }

        /// <summary>
        /// Determines the name of the remote folder and filename to transfer 
        /// </summary>
        /// <param name="zipEntity"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string CalculateRemoteFileName(ZipEntity zipEntity, ZipWatcherPreferences zipWatcherPreferences)
        {
            string fileName = Path.GetFileName(zipEntity.ZipFileName);
            return Path.Combine(zipWatcherPreferences.EndResultPath, DateTime.Now.GetYearMonthDay(), fileName ?? throw new InvalidOperationException());
        }


        /// <summary>
        /// Transfers files after they have been zipped to the remote share
        /// only one file with be transferred at a time
        /// </summary>
        /// <returns></returns>
        private async Task<bool> TransferringFiles()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                while (true)
                {
                    var zipEntity = _zipTranferQueue.Take();

                    if (!(Directory.Exists(zipEntity.RemoteOutputPath)))
                    {
                        Directory.CreateDirectory(zipEntity.RemoteOutputPath);
                    }
                    zipEntity.CopyZipStarted  = DateTime.Now;

                    AllLogWriter.Instance.LogMessage(
                        $"\nCopying file {zipEntity.ZipFileName} to {zipEntity.RemoteOutputFileName} ");
                    File.Copy(zipEntity.ZipFileName, zipEntity.RemoteOutputFileName, true);

                    //Finished Copying file
                    zipEntity.CopyZipFinished = DateTime.Now;
                    zipEntity.ZipCompressionDuration = zipEntity.CopyZipFinished - zipEntity.CopyZipStarted;

                    var ts = zipEntity.CopyZipDuration;
                    string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
                    AllLogWriter.Instance.LogMessage($"\nFinished zipping file for {zipEntity.BackupFileName} to {zipEntity.ZipFileName} duration:{elapsedTime}");
                }

            }
            catch (Exception ex)
            {
                AllLogWriter.Instance.LogErrorMessage(ex.ToString());
            }

            return false;
        }
    }
}