using FsInfoCat.Local;
using System;
using System.IO;

namespace FsInfoCat.Desktop
{
    public sealed class DirectoryImportEventArgs : FileSystemImportEventArgs
    {
        internal DirectoryImportEventArgs(FileSystemImporter.ScanContext scanContext, DirectoryInfo fsTarget, Subdirectory dbTarget, Exception error = null)
            : base(scanContext, fsTarget, dbTarget, error)
        {
        }

        public new DirectoryInfo FsTarget => (DirectoryInfo)base.FsTarget;
        public new Subdirectory DbTarget => (Subdirectory)base.DbTarget;
    }
}
