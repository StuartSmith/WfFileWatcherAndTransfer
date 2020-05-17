# WfFileWatcherAndTransfer
WfFileWatcherAndTransfer

---
A windows service that is used conjuction with SQL Server backups

After the back up run and copies the file down the to local System, this service will grab the file. Compress it using lzma compression ie 7zip and then transfer it to a remote file share. 

---

## Assumptions

This service will only watch for changes on one given folder. 


# App.config setting for service

**PathToWatch:** The path to watch for files being copied to. Example Value: D:\SQLBackups\

**PathTo7zip:** The path to where the 7zip executable file is located. Example Value: 7z.exe

**OutPutFolderFor7zip:** The path where 7zip back ups will be stored The path. Example Value: D:\SQLBackups\7zip

**NumberOfDaysTokeepZipBackups:** The number of days before the zip back ups will be remove: Example Value:1 

**RemoteServerWhereToCopyFiles:** Remote Server where to place the zip files once the back up is complete. The service will create a folder for each day it moves a back up For Example: 

\\RemoteShare
      ->20190401
      ->20190402
      ->20190403
      
      
