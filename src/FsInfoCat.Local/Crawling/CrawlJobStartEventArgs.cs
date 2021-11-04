using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobStartEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        [Obsolete("Use CrawlJob")]
        public ILocalCrawlConfiguration Configuration { get; }

        public ICrawlJob CrawlJob { get; }

        public bool IsFirstJob { get; }

        public CrawlJobStartEventArgs([DisallowNull] ICrawlJob crawlJob, bool isFirstJob, string message = null, StatusMessageLevel level = StatusMessageLevel.Information)
            : base(message, level, AsyncJobStatus.Running, (crawlJob ?? throw new ArgumentNullException(nameof(crawlJob))).ConcurrencyId)
        {
            CrawlJob = crawlJob;
            IsFirstJob = isFirstJob;
        }

        [Obsolete("Use CrawlJobStartEventArgs(ICrawlJob, bool, Guid[, string[, StatusMessageLevel]])")]
        public CrawlJobStartEventArgs([DisallowNull] ILocalCrawlConfiguration configuration, bool isFirstJob, Guid concurrencyId, string message = null, StatusMessageLevel level = StatusMessageLevel.Information)
            : base(message, level, AsyncJobStatus.Running, concurrencyId)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            IsFirstJob = isFirstJob;
        }

        public override string ToString() => $@"{GetType().ToCsTypeName(true)}
{{
  IsFirstJob = {ExtensionMethods.ToPseudoCsText(IsFirstJob).AsIndented()},
  {base.ToString()},
  Configuration = {ExtensionMethods.ToPseudoCsText(CrawlJob).AsIndented()}
}}";
    }
}
