using FsInfoCat.Local.Crawling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlManagerService : ICrawlManagerService
    {
        private LinkedList<WeakReference<ICrawlActivityEventListener>> _crawlActivityEventListeners = new();
        private LinkedList<WeakReference<ICrawlErrorEventListener>> _crawlErrorEventListeners = new();
        private LinkedList<WeakReference<ICrawlManagerEventListener>> _crawlManagerEventListeners = new();
        private LinkedList<WeakReference<IFileSystemItemEventListener>> _fileSystemItemEventListeners = new();
        private LinkedList<WeakReference<ISubdirectoryCrawlEventListener>> _subdirectoryCrawlEventListeners = new();
        private LinkedList<WeakReference<IFileCrawlEventListener>> _fileCrawlEventListeners = new();
        private LinkedList<CrawlJob> _pendingJobs = new();
        private CrawlJob _activeJob;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly ILogger<CrawlManagerService> _logger;

        public bool IsActive { get; private set; }

        private static IEnumerable<T> GetItems<T>(LinkedList<WeakReference<T>> list) where T : class
        {
            LinkedListNode<WeakReference<T>> node = list.First;
            while (node is not null)
            {
                LinkedListNode<WeakReference<T>> next = node.Next;
                if (node.Value.TryGetTarget(out T target))
                    yield return target;
                else
                    list.Remove(node);
                node = next;
            }
        }
        private static T[] GetItemArray<T>(LinkedList<WeakReference<T>> list)
            where T : class
        {
            T[] result;
            lock (list)
                result = GetItems(list).ToArray();
            return result;
        }
        private static bool AddItem<T>(LinkedList<WeakReference<T>> list, T item) where T : class
        {
            if (item is null)
                return false;
            lock (list)
            {
                if (GetItems(list).Any(i => ReferenceEquals(i, item)))
                    return false;
                list.AddLast(new WeakReference<T>(item));
            }
            return true;
        }
        private static bool RemoveItem<T>(LinkedList<WeakReference<T>> list, T item) where T : class
        {
            if (item is null)
                return false;
            lock (list)
            {
                LinkedListNode<WeakReference<T>> node = list.First;
                while (node is not null)
                {
                    LinkedListNode<WeakReference<T>> next = node.Next;
                    if (node.Value.TryGetTarget(out T target))
                    {
                        if (ReferenceEquals(target, item))
                        {
                            list.Remove(node);
                            return true;
                        }
                    }
                    else
                        list.Remove(node);
                    node = next;
                }
            }
            return false;
        }

        public void AddCrawlActivityEventListener([DisallowNull] ICrawlActivityEventListener listener) => AddItem(_crawlActivityEventListeners, listener);

        public bool RemoveCrawlActivityEventListener(ICrawlActivityEventListener listener) => RemoveItem(_crawlActivityEventListeners, listener);

        public void AddCrawlManagerEventListener([DisallowNull] ICrawlManagerEventListener listener) => AddItem(_crawlManagerEventListeners, listener);

        public bool RemoveCrawlManagerEventListener(ICrawlManagerEventListener listener) => RemoveItem(_crawlManagerEventListeners, listener);

        public void AddCrawlErrorEventListener([DisallowNull] ICrawlErrorEventListener listener) => AddItem(_crawlErrorEventListeners, listener);

        public bool RemoveCrawlErrorEventListener(ICrawlErrorEventListener listener) => RemoveItem(_crawlErrorEventListeners, listener);

        public void AddFileSystemItemEventListener([DisallowNull] IFileSystemItemEventListener listener) => AddItem(_fileSystemItemEventListeners, listener);

        public bool RemoveFileSystemItemEventListener(IFileSystemItemEventListener listener) => RemoveItem(_fileSystemItemEventListeners, listener);

        public void AddSubdirectoryCrawlEventListener([DisallowNull] ISubdirectoryCrawlEventListener listener) => AddItem(_subdirectoryCrawlEventListeners, listener);

        public bool RemoveSubdirectoryCrawlEventListener(ISubdirectoryCrawlEventListener listener) => RemoveItem(_subdirectoryCrawlEventListeners, listener);

        public void AddFileCrawlEventListener([DisallowNull] IFileCrawlEventListener listener) => AddItem(_fileCrawlEventListeners, listener);

        public bool RemoveFileCrawlEventListener(IFileCrawlEventListener listener) => RemoveItem(_fileCrawlEventListeners, listener);

        public ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt) => CrawlJob.StartAsync(this, crawlConfiguration, stopAt);

        public ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration) => CrawlJob.StartAsync(this, crawlConfiguration, null);

        public void CancelAllCrawlsAsync()
        {
            throw new NotImplementedException();
        }

        public CrawlManagerService(IFileSystemDetailService fileSystemDetailService, ILogger<CrawlManagerService> logger)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
        }

        private void RaiseCrawlJobStart(CrawlJobStartEventArgs args)
        {
            foreach (ICrawlActivityEventListener listener in GetItemArray(_crawlActivityEventListeners))
                listener.OnCrawlActivity(args);
            foreach (ICrawlManagerEventListener listener in GetItemArray(_crawlManagerEventListeners))
                listener.OnCrawlManagerEvent(args);
        }

        private void RaiseCrawlJobEnd(CrawlJobEndEventArgs args)
        {
            foreach (ICrawlActivityEventListener listener in GetItemArray(_crawlActivityEventListeners))
                listener.OnCrawlActivity(args);
            foreach (ICrawlManagerEventListener listener in GetItemArray(_crawlManagerEventListeners))
                listener.OnCrawlManagerEvent(args);
        }

        private void RaiseDirectoryCrawling(DirectoryCrawlEventArgs args)
        {

        }

        private void RaiseDirectoryCrawled(DirectoryCrawlEventArgs args)
        {

        }

        private void RaiseFileCrawling(FileCrawlEventArgs args)
        {

        }

        private void RaiseFileCrawled(FileCrawlEventArgs args)
        {

        }

        private static async Task CrawlAsync(CrawlJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static async Task CrawlNextAsync(CrawlManagerService service, CrawlJob job, CancellationToken cancellationToken)
        {
            lock (service._pendingJobs)
            {
                service._pendingJobs.Remove(job);
                service._activeJob = job;
            }
            await CrawlAsync(job, cancellationToken);
        }

        class CrawlWorker
        {
            private readonly CrawlManagerService _service;
            private readonly ushort _maxRecursionDepth;
            private readonly ulong? _maxTotalItems;
            private readonly long? _ttl;
            private readonly DateTime? _stopAt;
            private readonly CancellationToken _cancellationToken;
            private readonly Guid _rootId;

            internal ICurrentItem CurrentItem { get; private set; }

            internal string Message { get; private set; }

            internal StatusMessageLevel MessageLevel { get; private set; }

            internal TimeSpan Duration { get; private set; }

            internal CrawlWorker(CrawlManagerService service, ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt, CancellationToken cancellationToken)
            {
                _service = service;
                _maxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
                _maxTotalItems = crawlConfiguration.MaxTotalItems;
                _ttl = crawlConfiguration.TTL;
                _stopAt = stopAt;
                _cancellationToken = cancellationToken;
                _rootId = crawlConfiguration.RootId;
            }

            internal async Task RunAsync()
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Subdirectory subdirectory = await dbContext.Subdirectories.FindAsync(new object[] { _rootId }, _cancellationToken);
                if (subdirectory is null)
                    throw new Exception("Could not find crawl root subdirectory record");
                DirectoryInfo directoryInfo = await GetDirectoryInfoAsync(dbContext, subdirectory);
                if (directoryInfo is null)
                    throw new Exception("Could not find crawl root subdirectory volume");

            }

            private async Task<DirectoryInfo> GetDirectoryInfoAsync(LocalDbContext dbContext, Subdirectory subdirectory)
            {
                EntityEntry<Subdirectory> entry = dbContext.Entry(subdirectory);
                Subdirectory parent = await entry.GetRelatedReferenceAsync(d => d.Parent, _cancellationToken);
                if (parent is null)
                {
                    Volume volume = await entry.GetRelatedReferenceAsync(d => d.Volume, _cancellationToken);
                    if (volume is null)
                        return null;
                    ILogicalDiskInfo[] logicalDiskInfos = await _service._fileSystemDetailService.GetLogicalDisksAsync(_cancellationToken);
                    ILogicalDiskInfo matchingDiskInfo = logicalDiskInfos.FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volume.Identifier));
                    if (matchingDiskInfo is not null)
                        return new DirectoryInfo(matchingDiskInfo.Name);
                }
                else
                {
                    DirectoryInfo p = await GetDirectoryInfoAsync(dbContext, parent);
                    if (p is not null)
                        return new DirectoryInfo(Path.Combine(p.FullName, subdirectory.Name));
                }
                return null;
            }
        }
        class CrawlJob : ICrawlJob
        {
            private readonly CrawlManagerService _service;
            private readonly ILocalCrawlConfiguration _crawlConfiguration;
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly CrawlWorker _worker;

            private CrawlJob(CrawlManagerService service, ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt)
            {
                _service = service;
                _crawlConfiguration = crawlConfiguration;
                _worker = new(service, crawlConfiguration, stopAt, _tokenSource.Token);
                lock (service._pendingJobs)
                {
                    if (service._activeJob is null)
                    {
                        service._activeJob = this;
                        Task = _worker.RunAsync();
                    }
                    else
                        Task = (service._pendingJobs.AddLast(this).Previous?.Value ?? service._activeJob).Task
                            .ContinueWith(async t => await _worker.RunAsync());
                }
                Task.ContinueWith(t =>
                {
                });
            }

            public ICurrentItem CurrentItem { get; private set; }

            public string Title { get; private set; }

            public string Message { get; private set; }

            public StatusMessageLevel MessageLevel { get; private set; }

            public Guid ConcurrencyId { get; private set; }

            public AsyncJobStatus JobStatus { get; private set; }

            public Task Task { get; }

            public bool IsCancellationRequested { get; private set; }

            public TimeSpan Duration { get; private set; }

            public object AsyncState { get; private set; }

            public WaitHandle AsyncWaitHandle { get; private set; }

            public bool CompletedSynchronously { get; private set; }

            public bool IsCompleted { get; private set; }

            public void Cancel(bool throwOnFirstException)
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
    }
    public sealed partial class CrawlTaskManager : IDisposable
    {
        private bool _isDisposed;
        private readonly Stopwatch _stopWatch = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _task;
        private readonly Func<bool> _isExpired;
        private readonly ILogger<CrawlTaskManager> _logger;
        private readonly IFileSystemDetailService _fileSystemDetailService;

        public string StatusMessage { get; private set; } = "";

        public string DisplayName { get; }

        public ushort MaxRecursionDepth { get; }

        public ulong? MaxTotalItems { get; }

        public ulong TotalItems { get; private set; }

        public TimeSpan Elapsed => _stopWatch.Elapsed;

        public long ElapsedMilliseconds => _stopWatch.ElapsedMilliseconds;

        public long ElapsedTicks => _stopWatch.ElapsedTicks;

        public CrawlTaskManager([DisallowNull] CrawlConfiguration crawlConfiguration, DateTime stopAt, CrawlEventReceiver crawlEventReceiver = null)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<CrawlTaskManager>>();
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
            _ = _task.ContinueWith(OnCompleted, crawlEventReceiver);
        }

        public CrawlTaskManager([DisallowNull] CrawlConfiguration crawlConfiguration, CrawlEventReceiver crawlEventReceiver = null)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<CrawlTaskManager>>();
            _fileSystemDetailService = Services.ServiceProvider.GetRequiredService<IFileSystemDetailService>();
            DisplayName = crawlConfiguration.DisplayName;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            MaxTotalItems = crawlConfiguration.MaxTotalItems;
            long? ttl = crawlConfiguration.TTL;
            CancellationToken token = _cancellationTokenSource.Token;
            _isExpired = ttl.HasValue ? () => token.IsCancellationRequested || _stopWatch.ElapsedMilliseconds > ttl.Value : () => token.IsCancellationRequested;
            _task = CrawlAsync(crawlConfiguration, crawlEventReceiver, token);
            _ = _task.ContinueWith(OnCompleted, crawlEventReceiver);
        }

        private void OnCompleted(Task task, object arg)
        {
            CrawlEventReceiver crawlEventReceiver = (CrawlEventReceiver)arg;
            _stopWatch.Stop();
            if (task.IsCanceled)
            {
                StatusMessage = "Operation canceled.";
                _logger.LogWarning(StatusMessage);
                crawlEventReceiver?.RaiseCrawlCanceled(this);
            }
            else if (task.IsFaulted)
            {
                StatusMessage = "Operation failed.";
                _logger.LogError(ErrorCode.CrawlOperationFailed.ToEventId(), task.Exception, StatusMessage);
                crawlEventReceiver?.RaiseCrawlFaulted(this, task.Exception);
            }
            else
            {
                StatusMessage = "Operation Completed.";
                _logger.LogInformation(StatusMessage);
                crawlEventReceiver?.RaiseCrawlFinished(this);
            }
        }

        private async Task CrawlAsync(CrawlConfiguration configuration, CrawlEventReceiver crawlEventReceiver, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Crawl Started");
            crawlEventReceiver?.RaiseCrawlStarted(this);
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory subdirectory = await dbContext.Entry(configuration).GetRelatedReferenceAsync(c => c.Root, cancellationToken);
            if (subdirectory is null)
                throw new InvalidOperationException($"Unexpected error: {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)} was null.");
            string fullName = await subdirectory.GetFullNameAsync(dbContext, cancellationToken);
            if (string.IsNullOrEmpty(fullName))
                throw new InvalidOperationException($"Unexpected error: Could not build full path for {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)}.");
            await subdirectory.MarkBranchIncompleteAsync(dbContext, cancellationToken);
            CrawlContext context = new(this, 0, new DirectoryInfo(fullName), subdirectory);
            // TODO: Figure out how to deal with it if context.FS.Exists is false because volume is not present - or see if that could ever happen
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
    }
}
