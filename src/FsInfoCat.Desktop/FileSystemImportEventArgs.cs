using FsInfoCat.Local;
using System;
using System.IO;

namespace FsInfoCat.Desktop
{
    public abstract class FileSystemImportEventArgs : EventArgs
    {
        public FileSystemInfo FsTarget { get; }

        public ILocalDbFsItem DbTarget { get; }

        public Exception Fault { get; }

        public ushort RecursionDepth { get; }

        public ulong ItemCount { get; }

        public string DisplayName { get; }

        protected FileSystemImportEventArgs(FileSystemImportJob.ScanContext scanContext, FileSystemInfo fsTarget, ILocalDbFsItem dbTarget, Exception error)
        {
            Fault = error;
            RecursionDepth = scanContext.Depth;
            ItemCount = scanContext.TotalCount;
            DisplayName = scanContext.Job.DisplayName;
        }
    }
}
