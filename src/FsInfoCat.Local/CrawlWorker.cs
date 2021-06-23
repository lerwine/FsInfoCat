using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed class CrawlWorker : IDisposable
    {
        private bool _isDisposed;
        private readonly Stopwatch _stopWatch = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _task;
        private readonly Func<bool> _isExpired;
        private readonly IFileSystemDetailService _fileSystemDetailService;

        public string StatusMessage { get; private set; } = "";

        public string DisplayName { get; }

        public ushort MaxRecursionDepth { get; }

        public ulong MaxTotalItems { get; }

        public ulong TotalItems { get; private set; }

        public TimeSpan Elapsed => _stopWatch.Elapsed;

        public long ElapsedMilliseconds => _stopWatch.ElapsedMilliseconds;

        public long ElapsedTicks => _stopWatch.ElapsedTicks;

        public CrawlWorker([DisallowNull] CrawlConfiguration crawlConfiguration, DateTime stopAt, CrawlEventReceiver crawlEventReceiver = null)
        {
            _fileSystemDetailService = Services.ServiceProvider.GetRequiredService<IFileSystemDetailService>();
            DisplayName = crawlConfiguration.DisplayName;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            MaxTotalItems = crawlConfiguration.MaxTotalItems;
            CancellationToken token = _cancellationTokenSource.Token;
            long? ttl = crawlConfiguration.TTL;
            if (ttl.HasValue)
                _isExpired = () => token.IsCancellationRequested || _stopWatch.ElapsedMilliseconds > ttl.Value || DateTime.Now >= stopAt;
            else
                _isExpired = () => token.IsCancellationRequested || DateTime.Now >= stopAt;
            _task = CrawlAsync(crawlConfiguration, crawlEventReceiver, token);
            _task.ContinueWith(OnCompleted, crawlEventReceiver);
        }

        public CrawlWorker([DisallowNull] CrawlConfiguration crawlConfiguration, CrawlEventReceiver crawlEventReceiver = null)
        {
            _fileSystemDetailService = Services.ServiceProvider.GetRequiredService<IFileSystemDetailService>();
            DisplayName = crawlConfiguration.DisplayName;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            MaxTotalItems = crawlConfiguration.MaxTotalItems;
            long? ttl = crawlConfiguration.TTL;
            CancellationToken token = _cancellationTokenSource.Token;
            _isExpired = ttl.HasValue ? () => token.IsCancellationRequested || _stopWatch.ElapsedMilliseconds > ttl.Value : () => token.IsCancellationRequested;
            _task = CrawlAsync(crawlConfiguration, crawlEventReceiver, token);
            _task.ContinueWith(OnCompleted, crawlEventReceiver);
        }

        private void OnCompleted(Task task, object arg)
        {
            CrawlEventReceiver crawlEventReceiver = (CrawlEventReceiver)arg;
            _stopWatch.Stop();
            if (task.IsCanceled)
            {
                StatusMessage = "Operation canceled.";
                crawlEventReceiver?.RaiseCrawlCanceled(this);
            }
            else if (task.IsFaulted)
            {
                StatusMessage = "Operation failed.";
                crawlEventReceiver?.RaiseCrawlFaulted(this, task.Exception);
            }
            else
            {
                StatusMessage = "Operation Completed.";
                crawlEventReceiver?.RaiseCrawlFinished(this);
            }
        }

        private async Task CrawlAsync(CrawlConfiguration configuration, CrawlEventReceiver crawlEventReceiver, CancellationToken cancellationToken)
        {
            crawlEventReceiver?.RaiseCrawlStarted(this);
            using LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory subdirectory = await dbContext.Entry(configuration).GetRelatedReferenceAsync(c => c.Root, cancellationToken);
            if (subdirectory is null)
                throw new InvalidOperationException($"Unexpected error: {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)} was null.");
            string fullName = await Subdirectory.LookupFullNameAsync(subdirectory, dbContext);
            if (string.IsNullOrEmpty(fullName))
                throw new InvalidOperationException($"Unexpected error: Could not build full path for {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)}.");
            await subdirectory.MarkBranchIncompleteAsync(dbContext, cancellationToken);
            CrawlContext context = new(this, 0, new DirectoryInfo(fullName), subdirectory);
            await context.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        
        public class CrawlEventReceiver
        {
            public event EventHandler<CrawlEventArgs> CrawlStarted;
            public event EventHandler<CrawlEventArgs> CrawlCanceled;
            public event EventHandler<CrawlErrorEventArgs> CrawlFaulted;
            public event EventHandler<CrawlEventArgs> CrawlFinished;
            public event EventHandler<DirectoryCrawlEventArgs> EnterSubdirectory;
            public event EventHandler<DirectoryCrawlErrorEventArgs> SubdirectoryAccessError;
            public event EventHandler<DirectoryCrawlEventArgs> ExitSubdirectory;
            public event EventHandler<FileCrawlEventArgs> ReadingFile;
            public event EventHandler<FileCrawlErrorEventArgs> FileAccessError;
            public event EventHandler<FileCrawlEventArgs> FileReadComplete;
            internal void RaiseCrawlStarted(CrawlWorker crawlWorker) => OnCrawlStarted(new CrawlEventArgs(crawlWorker));

            internal void RaiseCrawlCanceled(CrawlWorker crawlWorker) => OnCrawlCanceled(new CrawlEventArgs(crawlWorker));

            internal void RaiseCrawlFaulted(CrawlWorker crawlWorker, AggregateException exception) => OnCrawlFaulted(new CrawlErrorEventArgs(crawlWorker, exception));

            internal void RaiseCrawlFinished(CrawlWorker crawlWorker) => OnCrawlFinished(new CrawlEventArgs(crawlWorker));

            internal void RaiseEnterSubdirectory(CrawlContext crawlContext) => OnEnterSubdirectory(new DirectoryCrawlEventArgs(crawlContext));

            internal void RaiseExitSubdirectory(CrawlContext crawlContext) => OnExitSubdirectory(new DirectoryCrawlEventArgs(crawlContext));

            internal void RaiseSubdirectoryAccessError(CrawlContext crawlContext, AccessErrorCode errorCode, Exception exception) =>
                OnSubdirectoryAccessErrory(new DirectoryCrawlErrorEventArgs(crawlContext, errorCode, exception));

            internal void RaiseReadingFile(CrawlContext crawlContext, FileInfo fs, DbFile db) => OnReadingFile(new FileCrawlEventArgs(crawlContext, fs, db));

            internal void RaiseFileReadComplete(CrawlContext crawlContext, FileInfo fs, DbFile db) => OnFileReadComplete(new FileCrawlEventArgs(crawlContext, fs, db));

            internal void RaiseFileAccessError(CrawlContext crawlContext, AccessErrorCode errorCode, FileInfo fs, DbFile db, Exception exception) =>
                OnFileAccessError(new FileCrawlErrorEventArgs(crawlContext, errorCode, fs, db, exception));

            private void OnCrawlStarted(CrawlEventArgs crawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnCrawlCanceled(CrawlEventArgs crawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnCrawlFaulted(CrawlErrorEventArgs crawlErrorEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnCrawlFinished(CrawlEventArgs crawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnEnterSubdirectory(DirectoryCrawlEventArgs directoryCrawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnExitSubdirectory(DirectoryCrawlEventArgs directoryCrawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnSubdirectoryAccessErrory(DirectoryCrawlErrorEventArgs directoryCrawlErrorEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnReadingFile(FileCrawlEventArgs fileCrawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnFileReadComplete(FileCrawlEventArgs fileCrawlEventArgs)
            {
                throw new NotImplementedException();
            }

            private void OnFileAccessError(FileCrawlErrorEventArgs fileCrawlErrorEventArgs)
            {
                throw new NotImplementedException();
            }
        }

        public class CrawlContext
        {
            public CrawlWorker Worker { get; }

            public int Depth { get; }

            public DirectoryInfo FS { get; }

            public Subdirectory DB { get; }

            internal CrawlContext([DisallowNull] CrawlWorker worker, int depth, [DisallowNull] DirectoryInfo fs, [DisallowNull] Subdirectory db)
            {
                Worker = worker ?? throw new ArgumentNullException(nameof(worker));
                Depth = depth;
                FS = fs ?? throw new ArgumentNullException(nameof(fs));
                DB = db ?? throw new ArgumentNullException(nameof(db));
            }

            internal async Task CrawlAsync([DisallowNull] LocalDbContext dbContext, [AllowNull] CrawlEventReceiver crawlEventReceiver, CancellationToken cancellationToken)
            {
                Worker.StatusMessage = $"Crawling {FS.FullName}";
                crawlEventReceiver?.RaiseEnterSubdirectory(this);
                EntityEntry<Subdirectory> dbEntry = dbContext.Entry(DB);
                if (FS.Exists)
                {
                    DB.LastAccessed = DateTime.Now;
                    FileInfo[] fsFiles = null;
                    try { fsFiles = FS.GetFiles(); }
                    catch (UnauthorizedAccessException exception)
                    {
                        DB.SetUnauthorizedAccessError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                    }
                    catch (SecurityException exception)
                    {
                        DB.SetSecurityError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                    }
                    catch (PathTooLongException exception)
                    {
                        DB.SetPathTooLongError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                    }
                    catch (IOException exception)
                    {
                        DB.SetIOError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                    }
                    catch (Exception exception)
                    {
                        DB.SetUnspecifiedError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                    DirectoryInfo[] fsDirectories = null;
                    if (Depth < Worker.MaxRecursionDepth)
                        try
                        {
                            DB.LastAccessed = DateTime.Now;
                            fsDirectories = FS.GetDirectories();
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.Status = DirectoryStatus.Complete;
                        }
                        catch (UnauthorizedAccessException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetUnauthorizedAccessError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                        }
                        catch (SecurityException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetSecurityError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                        }
                        catch (PathTooLongException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetPathTooLongError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                        }
                        catch (IOException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetIOError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                        }
                        catch (Exception exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetUnspecifiedError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                        }
                    if (fsFiles is not null)
                        foreach (var item in fsFiles.Join(await dbEntry.GetRelatedCollectionAsync(d => d.Files, cancellationToken), f => f.Name.ToLower(), f => f.Name.ToLower(),
                            (fs, db) => (FS: fs, DB: db)))
                        {
                            if (item.DB is null)
                            {
                                // TODO: Add new FsFile
                            }
                            else if (item.FS is null)
                                await item.DB.MarkDeletedAsync(dbContext, cancellationToken);
                            else
                            {
                                // TODO: Update item
                            }
                        }
                    if (Depth >= Worker.MaxRecursionDepth)
                    {
                        if (DB.Status == DirectoryStatus.Incomplete && fsFiles is not null)
                        {
                            DB.Status = DirectoryStatus.Complete;
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    else if (fsDirectories is not null)
                    {
                        foreach (var item in fsDirectories.Join(await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken), f => f.Name.ToLower(), f => f.Name.ToLower(),
                            (fs, db) => (FS: fs, DB: db)))
                        {
                            if (item.DB is null)
                            {
                                // TODO: Add new Subdirectory
                            }
                            else if (item.FS is null)
                            {
                                await item.DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
                                continue;
                            }
                            else
                            {
                                // TODO: Update item
                            }
                            CrawlContext context = new(Worker, Depth + 1, item.FS, item.DB);
                            await context.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
                        }
                        if (DB.Status == DirectoryStatus.Incomplete && fsFiles is not null)
                        {
                            DB.Status = DirectoryStatus.Complete;
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
                else
                    await DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
            }
        }
    }
    public class CrawlEventArgs : EventArgs
    {
        public CrawlEventArgs(CrawlWorker crawlWorker)
        {
        }

        public CrawlEventArgs(CrawlWorker.CrawlContext crawlContext)
        {
        }
    }
    public class DirectoryCrawlEventArgs : CrawlEventArgs
    {
        public DirectoryCrawlEventArgs(CrawlWorker.CrawlContext crawlContext) : base(crawlContext)
        {
        }
    }
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlErrorEventArgs(CrawlWorker.CrawlContext crawlContext, AccessErrorCode errorCode, Exception exception) : base(crawlContext)
        {
        }
    }
    public class FileCrawlEventArgs : CrawlEventArgs
    {
        public FileCrawlEventArgs(CrawlWorker.CrawlContext crawlContext, FileInfo fs, DbFile db) : base(crawlContext)
        {
        }
    }
    public class FileCrawlErrorEventArgs : FileCrawlEventArgs
    {
        public FileCrawlErrorEventArgs(CrawlWorker.CrawlContext crawlContext, AccessErrorCode errorCode, FileInfo fs, DbFile db, Exception exception)
            : base(crawlContext, fs, db)
        {
        }
    }
}
