using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Desktop
{
    public class FileSystemImportObserver : FileSystemImportJob.ScanContext.FileSystemImportObserverBase
    {
        public event EventHandler<FileSystemImportEventArgs> Importing;
        public event EventHandler<FileSystemImportEventArgs> Imported;
        public event EventHandler<FileSystemImportEventArgs> ImportError;
        public event EventHandler<FileImportEventArgs> FileImporting;
        public event EventHandler<FileImportEventArgs> FileImported;
        public event EventHandler<FileImportEventArgs> FileImportError;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImporting;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImported;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImportError;
        public event EventHandler JobStarted;
        public event EventHandler JobCanceled;
        public event EventHandler JobCompleted;

        protected override void OnDirectoryImporting(DirectoryImportEventArgs args)
        {
            try { Importing?.Invoke(this, args); }
            finally { DirectoryImporting?.Invoke(this, args); }
        }

        protected override void OnDirectoryImportError(DirectoryImportEventArgs args)
        {
            try { ImportError?.Invoke(this, args); }
            finally { DirectoryImportError?.Invoke(this, args); }
        }

        protected override void OnDirectoryImported(DirectoryImportEventArgs args)
        {
            try { Imported?.Invoke(this, args); }
            finally { DirectoryImported?.Invoke(this, args); }
        }

        protected override void OnFileImporting(FileImportEventArgs args)
        {
            try { Importing?.Invoke(this, args); }
            finally { FileImporting?.Invoke(this, args); }
        }

        protected override void OnFileImportError(FileImportEventArgs args)
        {
            try { ImportError?.Invoke(this, args); }
            finally { FileImportError?.Invoke(this, args); }
        }

        protected override void OnFileImported(FileImportEventArgs args)
        {
            try { Imported?.Invoke(this, args); }
            finally { FileImported?.Invoke(this, args); }
        }

        protected override void OnJobStarted() => JobStarted?.Invoke(this, EventArgs.Empty);

        protected override void OnJobCompleted() => JobCompleted?.Invoke(this, EventArgs.Empty);

        protected override void OnJobCanceled() => JobCanceled?.Invoke(this, EventArgs.Empty);
    }
}
