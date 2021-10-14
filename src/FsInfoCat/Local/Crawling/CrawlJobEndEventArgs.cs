using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobEndEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        public ILocalCrawlConfiguration Configuration { get; }
        public bool IsLastJob { get; }
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
  Configuration = {ExtensionMethods.ToPseudoCsText(Configuration).AsIndented()}
}}";
    }
}
