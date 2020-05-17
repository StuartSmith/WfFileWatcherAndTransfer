# WfFileWatcherAndTransfer
WfFileWatcherAndTransfer

---
A windows service that is used conjuction with SQL Server backups

After the back up run and copies the file down the to local System, this service will grab the file. Compress it using lzma compression ie 7zip and then transfer it to a remote file share. 

---

## Assumptions

This service will only watch for changes on one given folder. 


# App.config setting for service

- **WathToWatch:** The path to watch for files being copied to. Example Value: D:\SQLBackups\
- **PathTo7zip:** The path to where the 7zip executable file is located. Example Value: 7z.exe
- **OutPutFolderFor7zip:** The path where 7zip back ups will be stored The path. Example Value: D:\SQLBackups\7zip
- **NumberOfDaysTokeepZipBackups:** The number of days before the zip back ups will be remove: Example Value:1 
- **RemoteShareForZipFile:** The remote share where to copy the zip files after the compression step has completed. The files will be copied to a folder with the following format YYYYMMDD. If a file with the same names exists as the zip created the file file will be over written.  
    - \\\\RemoteShare
        - \20190401
        - \20190402
        - \20190403        
- **NUmberofDaysToKeepLogFiles:** Under the service there is a folder called Logs that stores a log file for each day there service has run.Example Value: 7
