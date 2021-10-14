using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobStartEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        public ILocalCrawlConfiguration Configuration { get; }
        public bool IsFirstJob { get; }
        public CrawlJobStartEventArgs(string message, StatusMessageLevel level, [DisallowNull] ILocalCrawlConfiguration configuration, bool isFirstJob, Guid concurrencyId)
            : base(message, level, AsyncJobStatus.Running, concurrencyId)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            IsFirstJob = isFirstJob;
        }

        public override string ToString() => $@"{GetType().ToCsTypeName(true)}
{{
  IsFirstJob = {ExtensionMethods.ToPseudoCsText(IsFirstJob).AsIndented()},
  {base.ToString()},
  Configuration = {ExtensionMethods.ToPseudoCsText(Configuration).AsIndented()}
}}";
    }
}
