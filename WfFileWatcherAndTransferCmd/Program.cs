using System;
using System.IO;
using WfFileWatcherAndTransferLib;
using WfFileWatcherAndTransferLib.Logging;

namespace WfFileWatcherAndTransferCmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ZipWatcherPreferences zInit = new ZipWatcherPreferences();
                zInit.PopulateFromConfigurationFile();

                string outputtext = "---------------------------------------------------------------------\n" +
                                    "Starting application for watching a specific folder \n" +
                                    "and then zipping the files added to said folder\n\n\n" +
                                    "File Watcher Parameters:\n" +
                                    $"Folder to Watch:           {zInit.PathToWatch}\n" +
                                    $"Zipping Output folder:     {zInit.OutPutFolderFor7zip}\n" +
                                    $"Path To 7zip:              {zInit.PathTo7Zip}\n" +
                                    $"Path To log file:          {Directory.GetCurrentDirectory()}\n" +
                                    "---------------------------------------------------------------------";

                AllLogWriter.Instance.LogMessage(outputtext);

                DirectoryZipWatcher dZipWatch = new DirectoryZipWatcher();
                dZipWatch.StartWatch(zInit);

                Console.WriteLine("");

                Console.WriteLine("Press Any Key to Continue");

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}