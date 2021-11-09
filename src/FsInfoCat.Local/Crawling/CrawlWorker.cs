using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    class CrawlProgressRelay : ICrawlMessageBus, IProgress<IAsyncOperationInfo>
    {
        private readonly object _syncRoot = new();
        private readonly ICrawlMessageBus _crawlMessageBus;

        public ICurrentItem CurrentItem { get; private set; }

        internal CrawlProgressRelay(ICrawlMessageBus crawlMessageBus) => _crawlMessageBus = crawlMessageBus;

        public void AddAsyncEventListener([DisallowNull] IProgress<IAsyncOperationInfo> listener) => _crawlMessageBus.AddAsyncEventListener(listener);

        public void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener) => _crawlMessageBus.AddCrawlActivityEventListener(listener);

        public void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener) => _crawlMessageBus.AddCrawlErrorEventListener(listener);

        public void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlJobEventArgs> listener) => _crawlMessageBus.AddCrawlManagerEventListener(listener);

        public void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents) => _crawlMessageBus.AddFileCrawlEventListener(listener, includeErrorEvents);

        public void AddFileSystemItemEventListener([DisallowNull] IProgress<IFsItemCrawlEventArgs> listener, bool includeErrorEvents) => _crawlMessageBus.AddFileSystemItemEventListener(listener, includeErrorEvents);

        public void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents) => _crawlMessageBus.AddSubdirectoryCrawlEventListener(listener, includeErrorEvents);

        public bool RemoveAsyncEventListener(IProgress<IAsyncOperationInfo> listener) => _crawlMessageBus.RemoveAsyncEventListener(listener);

        public bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener) => _crawlMessageBus.RemoveCrawlActivityEventListener(listener);

        public bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener) => _crawlMessageBus.RemoveCrawlErrorEventListener(listener);

        public bool RemoveCrawlManagerEventListener(IProgress<ICrawlJobEventArgs> listener) => _crawlMessageBus.RemoveCrawlManagerEventListener(listener);

        public bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener, bool includesErrorEvents) => _crawlMessageBus.RemoveFileCrawlEventListener(listener, includesErrorEvents);

        public bool RemoveFileSystemItemEventListener(IProgress<IFsItemCrawlEventArgs> listener, bool includesErrorEvents) => _crawlMessageBus.RemoveFileSystemItemEventListener(listener, includesErrorEvents);

        public bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener, bool includesErrorEvents) => _crawlMessageBus.RemoveSubdirectoryCrawlEventListener(listener, includesErrorEvents);

        public void Report(CrawlJobStartEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = null; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(CrawlJobEndEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = null; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(DirectoryCrawlStartEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(DirectoryCrawlEndEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value.Parent; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(DirectoryCrawlErrorEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value.Parent; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(FileCrawlStartEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(FileCrawlEndEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value.Parent; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        public void Report(FileCrawlErrorEventArgs value)
        {
            Monitor.Enter(_syncRoot);
            try { CurrentItem = value.Parent; }
            finally { Monitor.Exit(_syncRoot); }
            _crawlMessageBus.Report(value);
        }

        void IProgress<IAsyncOperationInfo>.Report(IAsyncOperationInfo value)
        {
            IAsyncOperationInfo eventArgs;
            if (CurrentItem is DirectoryCrawlEventArgs directoryCrawlEventArgs)
                eventArgs = new DirectoryCrawlEventArgs(directoryCrawlEventArgs, value);
            else if (CurrentItem is FileCrawlEventArgs fileCrawlEventArgs)
                eventArgs = new FileCrawlEventArgs(fileCrawlEventArgs, value);
            throw new NotImplementedException();
        }
    }
    partial class CrawlWorker : ICrawlSettings
    {
        private readonly ILogger<CrawlWorker> _logger;
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly ILocalCrawlConfiguration _crawlConfiguration;
        private readonly Guid _concurrencyId;

        public DateTime? StopAt { get; private set; }

        public TimeSpan? TTL { get; }

        public ICurrentItem CurrentItem { get; private set; }

        public long ItemsCrawled { get; private set; }

        public long FilesProcessed { get; private set; }

        public long FoldersProcessed { get; private set; }

        public string RootPath { get; private set; }

        public CrawlStatus StatusCode { get; private set; }

        public string StatusMessage { get; private set; }

        public string StatusDetail { get; private set; }

        public ushort MaxRecursionDepth { get; }

        public ulong? MaxTotalItems { get; }

        long? ICrawlSettings.TTL => TTL?.ToSeconds();

        internal CrawlWorker([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, Guid concurrencyId, DateTime? stopAt)
        {
            _concurrencyId = concurrencyId;
            long? ttl = (crawlConfiguration ?? throw new ArgumentNullException(nameof(crawlConfiguration))).TTL;
            TTL = ttl.HasValue ? TimeSpan.FromSeconds(ttl.Value) : null;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            MaxTotalItems = crawlConfiguration.MaxTotalItems;
            (_logger, _crawlMessageBus, _fileSystemDetailService, _crawlConfiguration, StopAt) = (Hosting.GetRequiredService<ILogger<CrawlWorker>>(), (crawlMessageBus ?? throw new ArgumentNullException(nameof(crawlMessageBus))),
                (fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService))), crawlConfiguration, stopAt);
            _logger.LogDebug($"{nameof(CrawlWorker)} instantiated");
        }

        internal async Task<bool?> DoWorkAsync([DisallowNull] IAsyncOperationProgress updateProgress)
        {
            CancellationToken cancellationToken = (updateProgress ?? throw new ArgumentNullException(nameof(updateProgress))).Token;
            _crawlMessageBus.Report
            long? ttl = _crawlConfiguration.TTL;
            if (ttl.HasValue)
            {
                DateTime stopAt = DateTime.Now.Add(TimeSpan.FromSeconds(ttl.Value));
                if (!(StopAt.HasValue && StopAt.Value.CompareTo(stopAt) < 0))
                    StopAt = stopAt;
            }

            if (StopAt.HasValue && StopAt.Value.CompareTo(DateTime.Now) <= 0)
                return false;
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = _crawlConfiguration.Id;
            EntityEntry<CrawlConfiguration> crawlConfiguration = dbContext.Entry(await dbContext.CrawlConfigurations.FirstOrDefaultAsync(c => c.Id == id, cancellationToken));
            Subdirectory subdirectory = await crawlConfiguration.GetRelatedReferenceAsync(d => d.Root, cancellationToken);
            if (StopAt.HasValue && StopAt.Value.CompareTo(DateTime.Now) <= 0)
                return false;
            if (subdirectory is null)
                throw new InvalidOperationException("Could not find subdirectory record for crawl configuration");
            System.IO.DirectoryInfo directoryInfo = await subdirectory.GetDirectoryInfoAsync(_fileSystemDetailService, dbContext, cancellationToken);
            return await CrawlAsync(new CurrentDirectory()
            {
                Target = directoryInfo,
                Entity = subdirectory
            }, cancellationToken);
        }

        // TODO: Implement CrawlWorker.CrawlAsync(CurrentDirectory, CancellationToken)
        // Return true if item limit reached; false if timeout, null if completed
        private async Task<bool?> CrawlAsync(CurrentDirectory currentDirectory, CancellationToken cancellationToken)
        {
            CurrentItem = currentDirectory;
            if (StopAt.HasValue && StopAt.Value.CompareTo(DateTime.Now) <= 0)
                return false;
            throw new NotImplementedException();
        }
    }
}
