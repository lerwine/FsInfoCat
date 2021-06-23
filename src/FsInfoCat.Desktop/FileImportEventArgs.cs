using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Desktop
{
    [Obsolete("Use FsInfoCat.Local.CrawlWorker, instead")]
    public sealed class FileImportEventArgs : FileSystemImportEventArgs
    {
        [NotNull]
        public new FileInfo FsTarget => (FileInfo)base.FsTarget;

        [MaybeNull]
        public new DbFile DbTarget => (DbFile)base.DbTarget;

        internal FileImportEventArgs([DisallowNull] FileSystemImportJob importJob, [DisallowNull] FileInfo fsTarget, [AllowNull] DbFile dbTarget, Exception error = null)
            : base(importJob, fsTarget, dbTarget, error)
        {
        }

        internal FileImportEventArgs([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] FileInfo fsTarget, [AllowNull] DbFile dbTarget, Exception error = null)
            : base(scanContext, fsTarget, dbTarget, error)
        {
        }
    }
}
