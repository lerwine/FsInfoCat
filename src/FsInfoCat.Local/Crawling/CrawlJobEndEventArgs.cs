using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobEndEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        public CrawlTerminationReason TerminationReason { get; }

        public ICrawlJob CrawlJob { get; }

        public bool IsLastJob { get; }

        public CrawlJobEndEventArgs([DisallowNull] ICrawlJob crawlJob, bool isLastJob, CrawlTerminationReason terminationReason)
            : base(crawlJob, terminationReason switch
            {
                CrawlTerminationReason.ItemLimitReached => MessageCode.ItemLimitReached,
                CrawlTerminationReason.TimeLimitReached => MessageCode.TimeLimitReached,
                CrawlTerminationReason.Aborted => (crawlJob.Status == AsyncJobStatus.Faulted) ? MessageCode.BackgroundJobFaulted : MessageCode.BackgroundJobCanceled,
                _ => crawlJob.StatusDescription,
            }, (crawlJob.Status == AsyncJobStatus.Faulted) ? crawlJob.CurrentOperation : null)
        {
            TerminationReason = terminationReason;
            CrawlJob = crawlJob;
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
