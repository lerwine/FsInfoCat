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
            long? ttl = crawlConfiguration.TTL;
            TTL = ttl.HasValue ? TimeSpan.FromSeconds(ttl.Value) : null;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            MaxTotalItems = crawlConfiguration.MaxTotalItems;
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
            Guid id = _crawlConfiguration.Id;
            DateTime crawlStart = DateTime.Now;
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
            throw new NotImplementedException();
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
