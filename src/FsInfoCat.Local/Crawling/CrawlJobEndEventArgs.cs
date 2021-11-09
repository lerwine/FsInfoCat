using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public sealed class CrawlJobEndEventArgs : CrawlActivityEventArgs, ICrawlJobEventArgs
    {
        public CrawlTerminationReason TerminationReason { get; }

        public ICrawlJob CrawlJob { get; }

        public bool IsLastJob { get; }

        // TODO: Create constructor that does not use ICrawlJob parameter
        public CrawlJobEndEventArgs([DisallowNull] ICrawlJob crawlJob, bool isLastJob, CrawlTerminationReason terminationReason)
            : base(crawlJob, (terminationReason == CrawlTerminationReason.Aborted) ? ((crawlJob.Status == AsyncJobStatus.Faulted) ? AsyncJobStatus.Faulted : AsyncJobStatus.Canceled) : AsyncJobStatus.Succeeded, terminationReason switch
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

        // TODO: Create constructor that does not use ICrawlJob parameter
        public CrawlJobEndEventArgs([DisallowNull] ICrawlJob crawlJob, bool isLastJob, AsyncOperationFailureException exception)
            : base(crawlJob, AsyncJobStatus.Faulted, ((IAsyncOperationInfo)exception).StatusDescription, exception.AsyncOperation.CurrentOperation)
        {
            TerminationReason = CrawlTerminationReason.Aborted;
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
