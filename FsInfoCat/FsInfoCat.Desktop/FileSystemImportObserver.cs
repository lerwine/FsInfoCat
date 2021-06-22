using FsInfoCat.Local;
using System;
using System.IO;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImportObserver
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

        internal void RaiseDirectoryImporting(FileSystemImportJob.ScanContext scanContext)
        {
            DirectoryImportEventArgs args = new DirectoryImportEventArgs(scanContext);
            Importing?.Invoke(this, args);
            DirectoryImporting?.Invoke(this, args);
        }

        internal void RaiseDirectoryImportError(FileSystemImportJob.ScanContext scanContext, Exception error)
        {
            DirectoryImportEventArgs args = new DirectoryImportEventArgs(scanContext, error);
            ImportError?.Invoke(this, args);
            DirectoryImportError?.Invoke(this, args);
        }

        internal void RaiseDirectoryImported(FileSystemImportJob.ScanContext scanContext)
        {
            DirectoryImportEventArgs args = new DirectoryImportEventArgs(scanContext);
            Imported?.Invoke(this, args);
            DirectoryImported?.Invoke(this, args);
        }

        internal void RaiseFileImporting(FileSystemImportJob.ScanContext scanContext, FileInfo fileInfo, DbFile dbFile)
        {
            throw new NotImplementedException();
        }

        internal void RaiseFileImportError(FileSystemImportJob.ScanContext scanContext, FileInfo fileInfo, DbFile dbFile, Exception error)
        {
            throw new NotImplementedException();
        }

        internal void RaiseFileImported(FileSystemImportJob.ScanContext scanContext, FileInfo fileInfo, DbFile dbFile)
        {
            throw new NotImplementedException();
        }
    }
}
