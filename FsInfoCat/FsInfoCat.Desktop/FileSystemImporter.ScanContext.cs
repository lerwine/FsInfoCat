using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImporter
    {
        public class ScanContext
        {
            private Stack<Queue<DirectoryInfo>> _stack = new();

            private ScanContext(FileSystemImporter importer, DirectoryInfo directoryInfo, Subdirectory subdirectory, CancellationToken cancellationToken)
            {
                Importer = importer;
                CurrentSubdirectory = subdirectory;
                CurrentDirectoryInfo = directoryInfo;
                CancellationToken = cancellationToken;
                DirectoryImportEventArgs args = new(this, directoryInfo, subdirectory);
                importer.Importing?.Invoke(this, args);
                importer.DirectoryImporting?.Invoke(this, args);
                try
                {
                    _files = new Queue<FileInfo>(directoryInfo.GetFiles());
                    _subdirectories = (importer.MaxRecursionDepth > 0) ? new(directoryInfo.GetDirectories()) : new();
                }
                catch (Exception error)
                {
                    args = new DirectoryImportEventArgs(this, directoryInfo, subdirectory, error);
                    importer?.ImportError(this, args);
                    importer?.DirectoryImportError(this, args);
                    return;
                }
                args = new DirectoryImportEventArgs(this, directoryInfo, subdirectory);
                importer.Imported?.Invoke(this, args);
                importer.DirectoryImported?.Invoke(this, args);
            }

            private Queue<FileInfo> _files = null;
            private Queue<DirectoryInfo> _subdirectories = null;

            public bool MoveNext(out FileInfo fileInfo)
            {
                if (_files.Count > 0)
                {
                    fileInfo = _files.Dequeue();
                    return true;
                }

                if (_subdirectories.Count == 0)
                {
                    fileInfo = null;
                    return false;
                }
                CurrentDirectoryInfo = _subdirectories.Dequeue();
                CurrentSubdirectory = null;
                _stack.Push(_subdirectories);
                _files = null;
                _subdirectories = null;
                try
                {
                    DirectoryImportEventArgs args = new(this, CurrentDirectoryInfo, CurrentSubdirectory);
                    Importer?.Importing(this, args);
                    Importer?.DirectoryImporting(this, args);
                    _files = new Queue<FileInfo>(CurrentDirectoryInfo.GetFiles());
                    _subdirectories = (_stack.Count < Importer.MaxRecursionDepth) ? new(CurrentDirectoryInfo.GetDirectories()) : new();
                }
                catch (Exception error)
                {
                    if (_files is null)
                        _files = new();
                    if (_subdirectories is null)
                        _subdirectories = new();
                    DirectoryImportEventArgs args = new(this, CurrentDirectoryInfo, CurrentSubdirectory, error);
                    Importer?.ImportError(this, args);
                    Importer?.DirectoryImportError(this, args);
                }
                return MoveNext(out fileInfo);
            }

            internal ushort Depth => (ushort)_stack.Count;
            internal Subdirectory CurrentSubdirectory { get; private set; }
            internal DirectoryInfo CurrentDirectoryInfo { get; private set; }
            internal CancellationToken CancellationToken { get; }
            internal ulong TotalCount { get; private set; }
            internal FileSystemImporter Importer { get; }
            internal static async Task<ScanContext> CreateAsync([NotNull] FileSystemImporter importer, LocalDbContext dbContext, CancellationToken cancellationToken) =>
                new ScanContext(importer, importer.DirectoryInfo, await GetSubdirectoryAsync(importer.DirectoryInfo, dbContext), cancellationToken);
        }
    }
}
