using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobEndEventArgs : CrawlActivityEventArgs, ICrawlManagerEventArgs
    {
        public ILocalCrawlConfiguration Configuration { get; }
        public bool IsLastJob { get; }
        public CrawlJobEndEventArgs(string message, StatusMessageLevel level, [DisallowNull] ILocalCrawlConfiguration configuration, bool isLastJob, Guid concurrencyId)
            : base(message, level, AsyncJobStatus.Running, concurrencyId)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            IsLastJob = isLastJob;
        }
    }
}
