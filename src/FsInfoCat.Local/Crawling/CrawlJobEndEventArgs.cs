using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobEndEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        [Obsolete("Use CrawlJob")]
        public ILocalCrawlConfiguration Configuration { get; }

        public CrawlTerminationReason TerminationReason { get; }

        public ICrawlJob CrawlJob { get; }

        public bool IsLastJob { get; }

        public CrawlJobEndEventArgs([DisallowNull] ICrawlJob crawlJob, bool isLastJob, CrawlTerminationReason terminationReason, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (crawlJob ?? throw new ArgumentNullException(nameof(crawlJob))).ConcurrencyId)
        {
            TerminationReason = terminationReason;
            CrawlJob = crawlJob;
            IsLastJob = isLastJob;
        }

        [Obsolete("Use CrawlJobEndEventArgs(ICrawlJob, bool, string, StatusMessageLevel)")]
        public CrawlJobEndEventArgs(string message, StatusMessageLevel level, [DisallowNull] ILocalCrawlConfiguration configuration, bool isLastJob, Guid concurrencyId)
            : base(message, level, AsyncJobStatus.Running, concurrencyId)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            IsLastJob = isLastJob;
        }

        public override string ToString() => $@"{GetType().ToCsTypeName(true)}
{{
  IsLastJob = {ExtensionMethods.ToPseudoCsText(IsLastJob).AsIndented()},
  {base.ToString()},
  Configuration = {ExtensionMethods.ToPseudoCsText(CrawlJob).AsIndented()}
}}";
    }
}
