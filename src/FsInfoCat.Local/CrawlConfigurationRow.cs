using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class CrawlConfigurationRow : LocalDbEntity, ILocalCrawlConfigurationRow, ISimpleIdentityReference<CrawlConfigurationRow>
    {
        #region Fields

        private Guid? _id;
        private string _displayName = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>Gets the display name.</summary>
        /// <value>The display name for the current crawl configuration.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        /// <summary>Gets the maximum recursion depth.</summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the <see cref="Root" /> <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

        // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
        /// <summary>Gets the maximum total items to crawl.</summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        public virtual ulong? MaxTotalItems { get; set; }

        // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
        /// <summary>Gets the maximum duration of the crawl.</summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>60</c>.</value>
        [Range(DbConstants.DbColMinValue_TTL_TotalSeconds, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TTLInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual long? TTL { get; set; }

        /// <summary>Gets the custom notes.</summary>
        /// <value>The custom notes to associate with the current crawl configuration.</value>
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>Gets a value indicating current crawl configuration status.</summary>
        /// <value>
        /// The <see cref="CrawlStatus" /> value that indicates the current status.
        /// </value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Status), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlStatus StatusValue { get; set; } = CrawlStatus.NotRunning;

        /// <summary>Gets the date and time when the last crawl was started.</summary>
        /// <value>The date and time when the last crawl was started or <see langword="null" /> if no crawl hhas ever been started for this configuration.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastCrawlStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? LastCrawlStart { get; set; }

        /// <summary>Gets the date and time when the last crawl was finshed.</summary>
        /// <value>
        /// The date and time when the last crawl was finshed; otherwise, <see langword="null" /> if the current crawl is still active or if
        /// no crawl has ever been started for this configuration.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastCrawlEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? LastCrawlEnd { get; set; }

        /// <summary>Gets the date and time when the next true is to begin.</summary>
        /// <value>The date and time when the next crawl is to begin or <see langword="null" /> if there is no scheduled crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_NextScheduledStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? NextScheduledStart { get; set; }

        // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
        /// <summary>Gets the length of time between automatic crawl re-scheduling.</summary>
        /// <value>The length of time between automatic crawl re-scheduling, in seconds or <cref langword="null" /> to disable automatic re-scheduling.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RescheduleInterval), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public long? RescheduleInterval { get; set; }

        /// <summary>Gets a value indicating whether automatic rescheduling is calculated from the completion time of the previous job, versus the start time.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the completion of the previous job;
        /// otherwise, <see langword="false" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the value
        /// of <see cref="NextScheduledStart" /> at the time the job is started.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RescheduleFromJobEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public bool RescheduleFromJobEnd { get; set; }

        /// <summary>Gets a value indicating whether crawl jobs are automatically rescheduled even if the previous job failed.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are always automatically re-scheduled; otherwise, <see langword="false" /> if crawl jobs are automatically
        /// re-scheduled only if the preceding job did not fail.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RescheduleAfterFail), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public bool RescheduleAfterFail { get; set; }

        public virtual Guid RootId { get; set; }

        CrawlConfigurationRow IIdentityReference<CrawlConfigurationRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        [Obsolete("Override property, instead")]
        protected virtual void OnRootIdChanged(Guid value) { }

        #endregion

        public TimeSpan? GetTTLAsTimeSpan()
        {
            long? value = TTL;
            return value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
        }

        public TimeSpan? GetRescheduleIntervalAsTimeSpan()
        {
            long? value = RescheduleInterval;
            return value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigurationRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlConfigurationRow other)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
