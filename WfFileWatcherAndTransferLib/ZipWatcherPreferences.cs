using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfFileWatcherAndTransferLib
{
    public class ZipWatcherPreferences
    {
        public string PathToWatch { get; set; }
        public string PathTo7Zip { get; set; }
        public string OutPutFolderFor7zip { get; set; }

        public void PopulateFromConfigurationFile()
        {
            PathToWatch = ConfigurationManager.AppSettings["PathToWatch"];
            PathTo7Zip = ConfigurationManager.AppSettings["PathTo7Zip"];
            OutPutFolderFor7zip = ConfigurationManager.AppSettings["OutPutFolderFor7zip"];
        }
    }
}
