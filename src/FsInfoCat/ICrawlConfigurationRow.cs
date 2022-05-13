using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface ICrawlConfigurationRow : IDbEntity, ICrawlSettings, IHasSimpleIdentifier
    {
        /// <summary>Gets the display name.</summary>
        /// <value>The display name for the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets the custom notes.</summary>
        /// <value>The custom notes to associate with the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        Guid RootId { get; }

        /// <summary>Gets a value indicating current crawl configuration status.</summary>
        /// <value>
        /// The <see cref="CrawlStatus" /> value that indicates the current status.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusValue { get; }

        /// <summary>Gets the date and time when the last crawl was started.</summary>
        /// <value>The date and time when the last crawl was started or <see langword="null" /> if no crawl hhas ever been started for this configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastCrawlStart), ResourceType = typeof(Properties.Resources))]
        DateTime? LastCrawlStart { get; }

        /// <summary>Gets the date and time when the last crawl was finshed.</summary>
        /// <value>
        /// The date and time when the last crawl was finshed; otherwise, <see langword="null" /> if the current crawl is still active or if
        /// no crawl has ever been started for this configuration.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastCrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? LastCrawlEnd { get; }

        /// <summary>Gets the date and time when the next true is to begin.</summary>
        /// <value>The date and time when the next crawl is to begin or <see langword="null" /> if there is no scheduled crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_NextScheduledStart), ResourceType = typeof(Properties.Resources))]
        DateTime? NextScheduledStart { get; }

        /// <summary>Gets the length of time between automatic crawl re-scheduling.</summary>
        /// <value>The length of time between automatic crawl re-scheduling, in seconds or <cref langword="null" /> to disable automatic re-scheduling.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleInterval), ResourceType = typeof(Properties.Resources))]
        long? RescheduleInterval { get; }

        /// <summary>Gets a value indicating whether automatic rescheduling is calculated from the completion time of the previous job, versus the start time.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the completion of the previous job;
        /// otherwise, <see langword="false" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the value
        /// of <see cref="NextScheduledStart" /> at the time the job is started.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleFromJobEnd), ResourceType = typeof(Properties.Resources))]
        bool RescheduleFromJobEnd { get; }

        /// <summary>Gets a value indicating whether crawl jobs are automatically rescheduled even if the previous job failed.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are always automatically re-scheduled; otherwise, <see langword="false" /> if crawl jobs are automatically
        /// re-scheduled only if the preceding job did not fail.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleAfterFail), ResourceType = typeof(Properties.Resources))]
        bool RescheduleAfterFail { get; }
    }
}
