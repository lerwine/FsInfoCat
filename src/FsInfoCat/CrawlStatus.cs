using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Repress the crawl status of a file.
    /// </summary>
    public enum CrawlStatus : byte
    {
        /// <summary>
        /// File system crawl is not running.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_NotRunning), ShortName = nameof(Properties.Resources.DisplayName_CrawlStatus_NotRunning),
            Description = nameof(Properties.Resources.Description_CrawlStatus_NotRunning), ResourceType = typeof(Properties.Resources))]
        NotRunning = 0,

        /// <summary>
        /// File system crawl is in progress.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_InProgress), ShortName = nameof(Properties.Resources.DisplayName_CrawlStatus_InProgress),
            Description = nameof(Properties.Resources.Description_CrawlStatus_InProgress), ResourceType = typeof(Properties.Resources))]
        InProgress = 1,

        /// <summary>
        /// File system crawl ran to completion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Completed), ShortName = nameof(Properties.Resources.DisplayName_Completed),
            Description = nameof(Properties.Resources.Description_CrawlStatus_Completed), ResourceType = typeof(Properties.Resources))]
        Completed = 2,

        /// <summary>
        /// File system was stopped prior to completion because the alotted execution duration had been reached.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_AllottedTimeElapsed), ShortName = nameof(Properties.Resources.DisplayName_Timeout),
            Description = nameof(Properties.Resources.Description_CrawlStatus_AllottedTimeElapsed), ResourceType = typeof(Properties.Resources))]
        AllottedTimeElapsed = 3,

        /// <summary>
        /// File system was stopped prior to completion because the maximum number of items had been processed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_MaxItemCountReached), ShortName = nameof(Properties.Resources.DisplayName_ItemLimit),
            Description = nameof(Properties.Resources.Description_CrawlStatus_MaxItemCountReached), ResourceType = typeof(Properties.Resources))]
        MaxItemCountReached = 4,

        /// <summary>
        /// File system crawl was manually canceled before completion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Canceled), ShortName = nameof(Properties.Resources.DisplayName_CrawlStatus_Canceled),
            Description = nameof(Properties.Resources.Description_CrawlStatus_Canceled), ResourceType = typeof(Properties.Resources))]
        Canceled = 5,

        /// <summary>
        /// File system crawl was aborted due to an unrecoverable failure.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Failed), ShortName = nameof(Properties.Resources.DisplayName_CrawlStatus_Failed),
            Description = nameof(Properties.Resources.Description_CrawlStatus_Failed), ResourceType = typeof(Properties.Resources))]
        Failed = 6,

        /// <summary>
        /// File system crawl configuration is disabled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Disabled), ShortName = nameof(Properties.Resources.DisplayName_CrawlStatus_Disabled),
            Description = nameof(Properties.Resources.Description_CrawlStatus_Disabled), ResourceType = typeof(Properties.Resources))]
        Disabled = 7
    }

}

