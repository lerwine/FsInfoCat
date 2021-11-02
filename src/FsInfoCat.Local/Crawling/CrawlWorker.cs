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
    partial class CrawlWorker
    {
        private readonly ILogger<CrawlWorker> _logger;
        private readonly ICrawlMessageBus _crawlMessageBus;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly ILocalCrawlConfiguration _crawlConfiguration;

        public DateTime? StopAt { get; private set; }

        public TimeSpan? TTL { get; }

        public ICurrentItem CurrentItem { get; internal set; }

        public long ItemsCrawled { get; internal set; }

        internal CrawlWorker([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, [DisallowNull] ICrawlMessageBus crawlMessageBus, [DisallowNull] IFileSystemDetailService fileSystemDetailService, DateTime? stopAt)
        {
            long? ttl = crawlConfiguration.TTL;
            TTL = ttl.HasValue ? TimeSpan.FromSeconds(ttl.Value) : null;
            (_logger, _crawlMessageBus, _fileSystemDetailService, _crawlConfiguration, StopAt) = (Hosting.GetRequiredService<ILogger<CrawlWorker>>(), crawlMessageBus, fileSystemDetailService, crawlConfiguration, stopAt);
            _logger.LogDebug($"{nameof(CrawlWorker)} instantiated");
        }

        internal async Task<bool?> DoWorkAsync(CancellationToken cancellationToken)
        {
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
            Subdirectory subdirectory = await dbContext.Subdirectories.FindAsync(_crawlConfiguration.RootId, cancellationToken);
            if (StopAt.HasValue && StopAt.Value.CompareTo(DateTime.Now) <= 0)
                return false;
            if (subdirectory is null)
                throw new InvalidOperationException("Could not find subdirectory record for crawl configuration");
            return await CrawlAsync(new CurrentDirectory()
            {
                Target = await subdirectory.GetDirectoryInfoAsync(_fileSystemDetailService, dbContext, cancellationToken),
                Entity = subdirectory
            }, cancellationToken);
        }

        // Return true if item limit reached; false if timeout, null if completed
        private async Task<bool?> CrawlAsync(CurrentDirectory currentDirectory, CancellationToken cancellationToken)
        {
            CurrentItem = currentDirectory;
            if (StopAt.HasValue && StopAt.Value.CompareTo(DateTime.Now) <= 0)
                return false;
            // TODO: Implement CrawlWorker.CrawlAsync
            throw new NotImplementedException();
        }
    }
}
