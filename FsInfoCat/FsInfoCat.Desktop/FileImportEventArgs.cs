using FsInfoCat.Local;
using System;
using System.IO;

namespace FsInfoCat.Desktop
{
    public sealed class FileImportEventArgs : FileSystemImportEventArgs
    {
        internal FileImportEventArgs(FileSystemImporter.ScanContext scanContext, FileInfo fsTarget, DbFile dbTarget, Exception error = null)
            : base(scanContext, fsTarget, dbTarget, error)
        {
        }

        public new FileInfo FsTarget => (FileInfo)base.FsTarget;
        public new DbFile DbTarget => (DbFile)base.DbTarget;
    }
}
