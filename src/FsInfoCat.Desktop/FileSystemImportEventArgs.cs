using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Desktop
{
    [Obsolete("Use FsInfoCat.Local.CrawlWorker, instead")]
    public abstract class FileSystemImportEventArgs : EventArgs
    {
        [NotNull]
        public FileSystemInfo FsTarget { get; }

        [MaybeNull]
        public ILocalDbFsItem DbTarget { get; }

        [MaybeNull]
        public Exception Fault { get; }

        public ushort RecursionDepth { get; }

        public ulong ItemCount { get; }

        [NotNull]
        public string DisplayName { get; }

        protected FileSystemImportEventArgs([DisallowNull] FileSystemImportJob importJob, [DisallowNull] FileSystemInfo fsTarget, [AllowNull] ILocalDbFsItem dbTarget, Exception error)
        {
            FsTarget = fsTarget ?? throw new ArgumentNullException(nameof(fsTarget));
            DbTarget = dbTarget;
            RecursionDepth = 0;
            ItemCount = 0;
            DisplayName = importJob.DisplayName;
            Fault = error;
        }

        protected FileSystemImportEventArgs([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] FileSystemInfo fsTarget, [AllowNull] ILocalDbFsItem dbTarget, Exception error)
        {
            FsTarget = fsTarget ?? throw new ArgumentNullException(nameof(fsTarget));
            RecursionDepth = (scanContext ?? throw new ArgumentNullException(nameof(scanContext))).Depth;
            DbTarget = dbTarget;
            ItemCount = scanContext.TotalCount;
            DisplayName = scanContext.Job.DisplayName;
            Fault = error;
        }
    }
}
