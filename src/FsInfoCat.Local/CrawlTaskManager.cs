using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
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

        public ulong MaxTotalItems { get; }

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
            _task.ContinueWith(OnCompleted, crawlEventReceiver);
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
            _task.ContinueWith(OnCompleted, crawlEventReceiver);
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
            using LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory subdirectory = await dbContext.Entry(configuration).GetRelatedReferenceAsync(c => c.Root, cancellationToken);
            if (subdirectory is null)
                throw new InvalidOperationException($"Unexpected error: {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)} was null.");
            string fullName = await Subdirectory.LookupFullNameAsync(subdirectory, dbContext);
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
