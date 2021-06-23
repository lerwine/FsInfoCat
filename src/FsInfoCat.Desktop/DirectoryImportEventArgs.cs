using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Desktop
{
    public sealed class DirectoryImportEventArgs : FileSystemImportEventArgs
    {
        [NotNull]
        public new DirectoryInfo FsTarget => (DirectoryInfo)base.FsTarget;

        [MaybeNull]
        public new Subdirectory DbTarget => (Subdirectory)base.DbTarget;

        internal DirectoryImportEventArgs([DisallowNull] FileSystemImportJob importJob, [DisallowNull] DirectoryInfo fsTarget, [AllowNull] Subdirectory dbTarget, Exception error = null)
            : base(importJob, fsTarget, dbTarget, error)
        {
        }

        internal DirectoryImportEventArgs([DisallowNull] FileSystemImportJob.ScanContext scanContext, Exception error = null)
            : base(scanContext, scanContext.FsDirectoryInfo, scanContext.DbDirectoryItem, error)
        {
        }
    }
}
