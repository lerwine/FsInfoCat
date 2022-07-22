using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents the crawl status of a file.
    /// </summary>
    public enum CrawlStatus : byte
    {
        /// <summary>
        /// File system crawl is not running.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.NotRunning), ShortName = nameof(Properties.Resources.NotRunning),
            Description = nameof(Properties.Resources.Description_NotRunning), ResourceType = typeof(Properties.Resources))]
        NotRunning = 0,

        /// <summary>
        /// File system crawl is in progress.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.InProgress), ShortName = nameof(Properties.Resources.InProgress),
            Description = nameof(Properties.Resources.CrawlInProgress), ResourceType = typeof(Properties.Resources))]
        InProgress = 1,

        /// <summary>
        /// File system crawl ran to completion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Completed), ShortName = nameof(Properties.Resources.Completed),
            Description = nameof(Properties.Resources.Description_CrawlCompleted), ResourceType = typeof(Properties.Resources))]
        Completed = 2,

        /// <summary>
        /// File system was stopped prior to completion because the alotted execution duration had been reached.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.AllottedExecutionTimeElapsed), ShortName = nameof(Properties.Resources.Timeout),
            Description = nameof(Properties.Resources.Description_AllottedExecutionTimeElapsed), ResourceType = typeof(Properties.Resources))]
        AllottedTimeElapsed = 3,

        /// <summary>
        /// File system was stopped prior to completion because the maximum number of items had been processed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.MaximumItemCountReached), ShortName = nameof(Properties.Resources.ItemLimit),
            Description = nameof(Properties.Resources.Description_MaximumItemCountReached), ResourceType = typeof(Properties.Resources))]
        MaxItemCountReached = 4,

        /// <summary>
        /// File system crawl was manually canceled before completion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CrawlJobCanceled), ShortName = nameof(Properties.Resources.CrawlJobCanceled),
            Description = nameof(Properties.Resources.Description_CrawlJobCanceled), ResourceType = typeof(Properties.Resources))]
        Canceled = 5,

        /// <summary>
        /// File system crawl was aborted due to an unrecoverable failure.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CrawlJobFailed), ShortName = nameof(Properties.Resources.CrawlJobFailed),
            Description = nameof(Properties.Resources.Description_CrawlJobFailed), ResourceType = typeof(Properties.Resources))]
        Failed = 6,

        /// <summary>
        /// File system crawl configuration is disabled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CrawlJobDisabled), ShortName = nameof(Properties.Resources.CrawlJobDisabled),
            Description = nameof(Properties.Resources.Description_CrawlJobDisabled), ResourceType = typeof(Properties.Resources))]
        Disabled = 7
    }
}

