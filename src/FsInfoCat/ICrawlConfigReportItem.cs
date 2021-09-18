namespace FsInfoCat
{
    public interface ICrawlConfigReportItem : ICrawlConfigurationListItem
    {
        long SucceededCount { get; }

        long TimedOutCount { get; }

        long ItemLimitReachedCount { get; }

        long CanceledCount { get; }

        long FailedCount { get; }

        long? AverageDuration { get; }

        long? MaxDuration { get; }
    }
}

