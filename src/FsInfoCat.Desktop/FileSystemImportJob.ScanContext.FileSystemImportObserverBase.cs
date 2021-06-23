using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImportJob
    {
        public partial class ScanContext
        {
            [Obsolete("Use FsInfoCat.Local.CrawlWorker, instead")]
            public abstract class FileSystemImportObserverBase
            {
                protected abstract void OnJobStarted();

                protected abstract void OnJobCompleted();

                protected abstract void OnJobCanceled();

                protected abstract void OnJobFailed(CrawlErrorEventArgs args);

                protected abstract void OnDirectoryImporting(DirectoryImportEventArgs args);

                protected abstract void OnDirectoryImportError(DirectoryImportEventArgs args);

                protected abstract void OnDirectoryImported(DirectoryImportEventArgs args);

                protected abstract void OnFileImporting(FileImportEventArgs args);

                protected abstract void OnFileImportError(FileImportEventArgs args);

                protected abstract void OnFileImported(FileImportEventArgs args);

                internal void RaiseJobStarted() => OnJobStarted();

                internal void RaiseJobCompleted() => OnJobCompleted();

                internal void RaiseJobCanceled() => OnJobCanceled();

                internal void RaiseDirectoryImporting([DisallowNull] FileSystemImportJob.ScanContext scanContext) => OnDirectoryImporting(new DirectoryImportEventArgs(scanContext));

                internal void RaiseDirectoryImportError([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] Exception error) => OnDirectoryImportError(new DirectoryImportEventArgs(scanContext, error));

                internal void RaiseDirectoryImported([DisallowNull] FileSystemImportJob.ScanContext scanContext) => OnDirectoryImported(new DirectoryImportEventArgs(scanContext));

                internal void RaiseFileImporting([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] FileInfo fileInfo, DbFile dbFile) => OnFileImporting(new FileImportEventArgs(scanContext, fileInfo, dbFile));

                internal void RaiseFileImportError([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] FileInfo fileInfo, [AllowNull] DbFile dbFile, [DisallowNull] Exception error) =>
                    OnFileImportError(new FileImportEventArgs(scanContext, fileInfo, dbFile, error));

                internal void RaiseFileImported([DisallowNull] FileSystemImportJob.ScanContext scanContext, [DisallowNull] FileInfo fileInfo, [DisallowNull] DbFile dbFile) => OnFileImported(new FileImportEventArgs(scanContext, fileInfo, dbFile));

                internal void RaiseJobFailed(AggregateException exception) => OnJobFailed(new CrawlErrorEventArgs(exception));
            }
        }
    }
}
