using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlWorker : IDisposable
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
    }
}
