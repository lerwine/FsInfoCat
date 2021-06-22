using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.LocalDb
{
    class SharedDbContext : ILocalDbContext
    {
        private static readonly object _syncRoot = new object();
        private static object _token;
        private static LocalDbContext _instance;
        private static SharedDbContext _latest;
        private bool _isDisposed;
        private SharedDbContext _previous;
        private SharedDbContext _next;

        internal SharedDbContext()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if ((_previous = _latest) is null)
                {
                    _token = new object();
                    if (_instance is null)
                        _instance = new LocalDbContext();
                    _latest = this;
                }
                else
                    _latest = _previous._next = this;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        private static void HandleCloseTask(object token)
        {
            LocalDbContext dbContext;
            Thread.Sleep(DBSettings.Default.KeepLocalDbAlive);
            Monitor.Enter(_syncRoot);
            try
            {
                if (ReferenceEquals(_token, token))
                {
                    dbContext = _instance;
                    _instance = null;
                }
                else
                    return;
            }
            finally { Monitor.Exit(_syncRoot); }
            dbContext.Dispose();
        }

        public void Dispose()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_isDisposed)
                    return;
                _isDisposed = true;
                if (_next is null)
                {
                    if ((_latest = _previous) is null)
                        Task.Factory.StartNew(HandleCloseTask, _token);
                }
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public IQueryable<ILocalContentInfo> ContentInfos => _instance.ContentInfos;

        public IQueryable<ILocalFileComparison> Comparisons => _instance.Comparisons;

        public IQueryable<ILocalFile> Files => _instance.Files;

        public IQueryable<ILocalSubDirectory> Subdirectories => _instance.Directories;

        public IQueryable<ILocalVolume> Volumes => _instance.Volumes;

        public IQueryable<ILocalSymbolicName> SymbolicNames => _instance.SymbolicNames;

        public IQueryable<ILocalFileSystem> FileSystems => _instance.FileSystems;

        public IQueryable<ILocalRedundancy> Redundancies => _instance.Redundancies;

        public IQueryable<ILocalRedundantSet> RedundantSets => _instance.RedundantSets;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        IQueryable<IFsSymbolicName> IDbContext.SymbolicNames => SymbolicNames;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories;

        IQueryable<IFile> IDbContext.Files => Files;

        IQueryable<IContentInfo> IDbContext.ContentInfos => ContentInfos;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<IRedundantSet> IDbContext.RedundantSets => RedundantSets;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        public IDbContextTransaction BeginTransaction() => _instance.Database.BeginTransaction();

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) => _instance.Database.BeginTransaction(isolationLevel);

        public bool HasChanges() => _instance.ChangeTracker.HasChanges();

        internal EntityEntry<ContentInfo> AddContentInfo(ContentInfo contentInfo)
        {
            _instance.ContentInfos.Attach(contentInfo ?? throw new ArgumentNullException(nameof(contentInfo)));
            return _instance.ContentInfos.Add(contentInfo);
        }

        internal EntityEntry<ContentInfo> UpdateContentInfo(ContentInfo contentInfo)
        {
            _instance.ContentInfos.Attach(contentInfo ?? throw new ArgumentNullException(nameof(contentInfo)));
            return _instance.ContentInfos.Update(contentInfo);
        }

        internal EntityEntry<ContentInfo> RemoveContentInfo(ContentInfo contentInfo)
        {
            _instance.ContentInfos.Attach(contentInfo ?? throw new ArgumentNullException(nameof(contentInfo)));
            return _instance.ContentInfos.Remove(contentInfo);
        }

        internal EntityEntry<FileComparison> AddComparison(FileComparison fileComparison)
        {
            _instance.Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return _instance.Comparisons.Add(fileComparison);
        }

        internal EntityEntry<FileComparison> UpdateComparison(FileComparison fileComparison)
        {
            _instance.Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return _instance.Comparisons.Update(fileComparison);
        }

        internal EntityEntry<FileComparison> RemoveComparison(FileComparison fileComparison)
        {
            _instance.Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return _instance.Comparisons.Remove(fileComparison);
        }

        internal EntityEntry<FsFile> AddFile(FsFile file)
        {
            _instance.Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return _instance.Files.Add(file);
        }

        internal EntityEntry<FsFile> UpdateFile(FsFile file)
        {
            _instance.Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return _instance.Files.Update(file);
        }

        internal EntityEntry<FsFile> RemoveFile(FsFile file)
        {
            _instance.Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return _instance.Files.Remove(file);
        }

        internal EntityEntry<FsDirectory> AddSubDirectory(FsDirectory subdirectory)
        {
            _instance.Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return _instance.Directories.Add(subdirectory);
        }

        internal EntityEntry<FsDirectory> UpdateSubDirectory(FsDirectory subdirectory)
        {
            _instance.Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return _instance.Directories.Update(subdirectory);
        }

        internal EntityEntry<FsDirectory> RemoveSubDirectory(FsDirectory subdirectory)
        {
            _instance.Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return _instance.Directories.Remove(subdirectory);
        }

        internal EntityEntry<Volume> AddVolume(Volume volume)
        {
            _instance.Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return _instance.Volumes.Add(volume);
        }

        internal EntityEntry<Volume> UpdateVolume(Volume volume)
        {
            _instance.Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return _instance.Volumes.Update(volume);
        }

        internal EntityEntry<Volume> RemoveVolume(Volume volume)
        {
            _instance.Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return _instance.Volumes.Remove(volume);
        }

        internal EntityEntry<SymbolicName> AddSymbolicName(SymbolicName symbolicName)
        {
            _instance.SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return _instance.SymbolicNames.Add(symbolicName);
        }

        internal EntityEntry<SymbolicName> UpdateSymbolicName(SymbolicName symbolicName)
        {
            _instance.SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return _instance.SymbolicNames.Update(symbolicName);
        }

        internal EntityEntry<SymbolicName> RemoveSymbolicName(SymbolicName symbolicName)
        {
            _instance.SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return _instance.SymbolicNames.Remove(symbolicName);
        }

        internal EntityEntry<FileSystem> AddFileSystem(FileSystem fileSystem)
        {
            _instance.FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return _instance.FileSystems.Add(fileSystem);
        }

        internal EntityEntry<FileSystem> UpdateFileSystem(FileSystem fileSystem)
        {
            _instance.FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return _instance.FileSystems.Update(fileSystem);
        }

        internal EntityEntry<FileSystem> RemoveFileSystem(FileSystem fileSystem)
        {
            _instance.FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return _instance.FileSystems.Remove(fileSystem);
        }

        internal EntityEntry<Redundancy> AddRedundancy(Redundancy redundancy)
        {
            _instance.Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return _instance.Redundancies.Add(redundancy);
        }

        internal EntityEntry<Redundancy> UpdateRedundancy(Redundancy redundancy)
        {
            _instance.Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return _instance.Redundancies.Update(redundancy);
        }

        internal EntityEntry<Redundancy> RemoveRedundancy(Redundancy redundancy)
        {
            _instance.Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return _instance.Redundancies.Remove(redundancy);
        }

        public void AddHashCalculation(ILocalContentInfo hashCalculation)
        {
            throw new NotImplementedException();
        }

        public void UpdateHashCalculation(ILocalContentInfo hashCalculation)
        {
            throw new NotImplementedException();
        }

        public void RemoveHashCalculation(ILocalContentInfo hashCalculation)
        {
            throw new NotImplementedException();
        }

        public void AddComparison(ILocalFileComparison fileComparison)
        {
            throw new NotImplementedException();
        }

        public void UpdateComparison(ILocalFileComparison fileComparison)
        {
            throw new NotImplementedException();
        }

        public void RemoveComparison(ILocalFileComparison fileComparison)
        {
            throw new NotImplementedException();
        }

        public void AddFile(ILocalFile file)
        {
            throw new NotImplementedException();
        }

        public void UpdateFile(ILocalFile file)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(ILocalFile file)
        {
            throw new NotImplementedException();
        }

        public void AddSubDirectory(ILocalSubDirectory subDirectory)
        {
            throw new NotImplementedException();
        }

        public void UpdateSubDirectory(ILocalSubDirectory subDirectory)
        {
            throw new NotImplementedException();
        }

        public void RemoveSubDirectory(ILocalSubDirectory subDirectory)
        {
            throw new NotImplementedException();
        }

        public void AddVolume(ILocalVolume volume)
        {
            throw new NotImplementedException();
        }

        public void UpdateVolume(ILocalVolume volume)
        {
            throw new NotImplementedException();
        }

        public void RemoveVolume(ILocalVolume volume)
        {
            throw new NotImplementedException();
        }

        public void AddSymbolicName(ILocalSymbolicName symbolicName)
        {
            throw new NotImplementedException();
        }

        public void UpdateSymbolicName(ILocalSymbolicName symbolicName)
        {
            throw new NotImplementedException();
        }

        public void RemoveSymbolicName(ILocalSymbolicName symbolicName)
        {
            throw new NotImplementedException();
        }

        public void AddFileSystem(ILocalFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        public void UpdateFileSystem(ILocalFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        public void RemoveFileSystem(ILocalFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        public void AddRedundancy(ILocalRedundancy redundancy)
        {
            throw new NotImplementedException();
        }

        public void UpdateRedundancy(ILocalRedundancy redundancy)
        {
            throw new NotImplementedException();
        }

        public void RemoveRedundancy(ILocalRedundancy redundancy)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
