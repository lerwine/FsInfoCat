using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobStartEventArgs : CrawlActivityEventArgs, ICrawlManagerEventArgs
    {
        public ILocalCrawlConfiguration Configuration { get; }
        public bool IsFirstJob { get; }
        public CrawlJobStartEventArgs(string message, StatusMessageLevel level, [DisallowNull] ILocalCrawlConfiguration configuration, bool isFirstJob, Guid concurrencyId)
            : base(message, level, AsyncJobStatus.Running, concurrencyId)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            IsFirstJob = isFirstJob;
        }
    }
}
