using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobStartEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        public ICrawlJob CrawlJob { get; }

        public bool IsFirstJob { get; }

        public CrawlJobStartEventArgs([DisallowNull] ICrawlJob crawlJob, bool isFirstJob, string message = null, StatusMessageLevel level = StatusMessageLevel.Information)
            : base(message, level, AsyncJobStatus.Running, (crawlJob ?? throw new ArgumentNullException(nameof(crawlJob))).ConcurrencyId)
        {
            CrawlJob = crawlJob;
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
