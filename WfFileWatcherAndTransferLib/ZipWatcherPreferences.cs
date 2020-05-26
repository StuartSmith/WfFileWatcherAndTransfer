using System;
using System.Configuration;
using System.IO;
using WfFileWatcherAndTransferLib.Logging;

namespace WfFileWatcherAndTransferLib
{
    public class ZipWatcherPreferences
    {
        public string PathToWatch { get; set; }
        public string PathTo7Zip { get; set; }
        public string OutPutFolderFor7zip { get; set; }
        public string EndResultPath { get; set; }

        public void PopulateFromConfigurationFile()
        {
            try
            {
                bool failedToFindFolder = false;
                string failedToFind = "";

                PathToWatch = ConfigurationManager.AppSettings["PathToWatch"];
                PathTo7Zip = ConfigurationManager.AppSettings["PathTo7Zip"];
                OutPutFolderFor7zip = ConfigurationManager.AppSettings["OutPutFolderFor7zip"];
                EndResultPath = ConfigurationManager.AppSettings["EndResultPath"];

                if (!Directory.Exists(PathToWatch))
                {
                    failedToFindFolder = true;
                    failedToFind =
                        $"{failedToFind} \n Could not find Path to Watch for changes {PathToWatch}";
                }

                string SevenZipPath = Path.Combine(PathTo7Zip, "7z.exe");
                if (!File.Exists(SevenZipPath))
                {
                    failedToFindFolder = true;
                    failedToFind =
                        $"{failedToFind} \n Could not find 7z.exe {SevenZipPath}";
                }

                if (failedToFindFolder)
                {
                    AllLogWriter.Instance.LogErrorMessage(failedToFind);
                    throw new Exception(failedToFind);
                }

                Directory.CreateDirectory(EndResultPath);
                if (!Directory.Exists(EndResultPath))
                {
                    string msg = $"Could not find the EndResultPath {EndResultPath}";
                    AllLogWriter.Instance.LogErrorMessage(msg);
                }
            }
            catch (Exception ex)
            {
                AllLogWriter.Instance.LogErrorMessage(ex.ToString());
                throw;
            }
        }
    }
}