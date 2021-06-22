using FsInfoCat.Local;
using System;
using System.IO;

namespace FsInfoCat.Desktop
{
    public sealed class DirectoryImportEventArgs : FileSystemImportEventArgs
    {
        internal DirectoryImportEventArgs(FileSystemImportJob.ScanContext scanContext, Exception error = null)
            : base(scanContext, scanContext.FsDirectoryInfo, scanContext.DbDirectoryItem, error)
        {
        }
        public new DirectoryInfo FsTarget => (DirectoryInfo)base.FsTarget;
        public new Subdirectory DbTarget => (Subdirectory)base.DbTarget;
    }
}
