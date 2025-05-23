using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for a crawl configuration entity.
/// </summary>
/// <seealso cref="CrawlConfiguration" />
/// <seealso cref="CrawlConfigListItemBase" />
/// <seealso cref="LocalDbContext.CrawlConfigurations" />
/// <seealso cref="LocalDbContext.CrawlConfigListing" />
/// <seealso cref="LocalDbContext.CrawlConfigReport" />
public abstract class CrawlConfigurationRow : LocalDbEntity, ILocalCrawlConfigurationRow
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
    [BackingField(nameof(_id))]
    [Display(Name = nameof(FsInfoCat.Properties.Resources.UniqueIdentifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

    /// <summary>
    /// Gets the display name.
    /// </summary>
    /// <value>The display name for the current crawl configuration.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
        ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
    [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
        ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_displayName))]
    public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the maximum recursion depth.
    /// </summary>
    /// <value>
    /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the root <see cref="ISubdirectory" />,
    /// a value will crawl 1 sub-folder deep, and so on.
    /// </value>
    [Required]
    [Display(Name = nameof(FsInfoCat.Properties.Resources.MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public virtual ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

    // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
    /// <summary>
    /// Gets the maximum total items to crawl.
    /// </summary>
    /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
    public virtual ulong? MaxTotalItems { get; set; }

    // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
    /// <summary>
    /// Gets the maximum duration of the crawl.
    /// </summary>
    /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>60</c>.</value>
    [Range(DbConstants.DbColMinValue_TTL_TotalSeconds, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TTLInvalid),
        ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
    public virtual long? TTL { get; set; }

    /// <summary>
    /// Gets the custom notes.
    /// </summary>
    /// <value>The custom notes to associate with the current crawl configuration.</value>
    [Required(AllowEmptyStrings = true)]
    [NotNull]
    [BackingField(nameof(_notes))]
    public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Gets a value indicating current crawl configuration status.
    /// </summary>
    /// <value>
    /// The <see cref="CrawlStatus" /> value that indicates the current status.
    /// </value>
    [Required]
    [Display(Name = nameof(FsInfoCat.Properties.Resources.StatusValue), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public CrawlStatus StatusValue { get; set; } = CrawlStatus.NotRunning;

    /// <summary>
    /// Gets the date and time when the last crawl was started.
    /// </summary>
    /// <value>The date and time when the last crawl was started or <see langword="null" /> if no crawl hhas ever been started for this configuration.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.LastCrawlStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? LastCrawlStart { get; set; }

    /// <summary>
    /// Gets the date and time when the last crawl was finshed.
    /// </summary>
    /// <value>
    /// The date and time when the last crawl was finshed; otherwise, <see langword="null" /> if the current crawl is still active or if
    /// no crawl has ever been started for this configuration.
    /// </value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.LastCrawlEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? LastCrawlEnd { get; set; }

    /// <summary>
    /// Gets the date and time when the next true is to begin.
    /// </summary>
    /// <value>The date and time when the next crawl is to begin or <see langword="null" /> if there is no scheduled crawl.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.NextScheduledStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? NextScheduledStart { get; set; }

    // DEFERRED: Not sure if Range would work here for a nullable value, but minimum value needs to be validated.
    /// <summary>
    /// Gets the length of time between automatic crawl re-scheduling.
    /// </summary>
    /// <value>The length of time between automatic crawl re-scheduling, in seconds or <see langword="null" /> to disable automatic re-scheduling.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.RescheduleInterval), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public long? RescheduleInterval { get; set; }

    /// <summary>
    /// Gets a value indicating whether automatic rescheduling is calculated from the completion time of the previous job, versus the start time.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the completion of the previous job;
    /// otherwise, <see langword="false" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the value
    /// of <see cref="NextScheduledStart" /> at the time the job is started.
    /// </value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.RescheduleFromJobEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public bool RescheduleFromJobEnd { get; set; }

    /// <summary>
    /// Gets a value indicating whether crawl jobs are automatically rescheduled even if the previous job failed.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if crawl jobs are always automatically re-scheduled; otherwise, <see langword="false" /> if crawl jobs are automatically
    /// re-scheduled only if the preceding job did not fail.
    /// </value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.RescheduleAfterFail), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public bool RescheduleAfterFail { get; set; }

    /// <summary>
    /// Gets the primary key of the root directory entity.
    /// </summary>
    /// <value>The primary key value of the root <see cref="Subdirectory"/> entity.</value>
    public abstract Guid RootId { get; set; }

    #endregion

    /// <summary>
    /// Gets the <see cref="TTL"/> property value as a <see cref="TimeSpan"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> value from the number of seconds specified by the <see cref="TTL"/> property or <see langword="null"/> if <see cref="TTL"/> is <see langword="null"/>.</returns>
    public TimeSpan? GetTTLAsTimeSpan()
    {
        long? value = TTL;
        return value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
    }

    /// <summary>
    /// Gets the <see cref="RescheduleInterval"/> property value as a <see cref="TimeSpan"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> value from the number of seconds specified by the <see cref="RescheduleInterval"/> property or <see langword="null"/> if <see cref="RescheduleInterval"/> is <see langword="null"/>.</returns>
    public TimeSpan? GetRescheduleIntervalAsTimeSpan()
    {
        long? value = RescheduleInterval;
        return value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
    }

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalCrawlConfigurationRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigurationRow other) => ArePropertiesEqual((ICrawlConfigurationRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ICrawlConfigurationRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlConfigurationRow other) => CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn &&
        _displayName == other.DisplayName &&
        _notes == other.Notes &&
        MaxRecursionDepth == other.MaxRecursionDepth &&
        MaxTotalItems == other.MaxTotalItems &&
        TTL == other.TTL &&
        StatusValue == other.StatusValue &&
        LastCrawlStart == other.LastCrawlStart &&
        LastCrawlEnd == other.LastCrawlEnd &&
        NextScheduledStart == other.NextScheduledStart &&
        RescheduleInterval == other.RescheduleInterval &&
        RescheduleFromJobEnd == other.RescheduleFromJobEnd &&
        RescheduleAfterFail == other.RescheduleAfterFail;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected virtual string PropertiesToString() => $@"StatusValue={StatusValue}, MaxRecursionDepth={MaxRecursionDepth}, MaxTotalItems={MaxTotalItems}, TTL={TTL},
    LastCrawlStart={LastCrawlStart:yyyy-mm-ddTHH:mm:ss.fffffff}, LastCrawlEnd={LastCrawlEnd:yyyy-mm-ddTHH:mm:ss.fffffff}, NextScheduledStart={NextScheduledStart:yyyy-mm-ddTHH:mm:ss.fffffff},
    RescheduleInterval={RescheduleInterval}, RescheduleFromJobEnd={RescheduleFromJobEnd}, RescheduleAfterFail={RescheduleAfterFail}";

    public override string ToString() => $@"{{ Id={_id}, DisplayName=""{ExtensionMethods.EscapeCsString(_displayName)}"", {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    Notes=""{ExtensionMethods.EscapeCsString(_notes)}"" }}";

    public override int GetHashCode()
    {
        Guid? id = _id;
        if (id.HasValue) return id.Value.GetHashCode();
        HashCode hash = new();
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(_displayName);
        hash.Add(_notes);
        hash.Add(MaxRecursionDepth);
        hash.Add(MaxTotalItems);
        hash.Add(TTL);
        hash.Add(StatusValue);
        hash.Add(LastCrawlStart);
        hash.Add(LastCrawlEnd);
        hash.Add(NextScheduledStart);
        hash.Add(RescheduleInterval);
        hash.Add(RescheduleFromJobEnd);
        hash.Add(RescheduleAfterFail);
        return hash.ToHashCode();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Gets the unique identifier of the current entity if it has been assigned.
    /// </summary>
    /// <param name="result">Receives the unique identifier value.</param>
    /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
    public bool TryGetId(out Guid result)
    {
        Guid? id = _id;
        if (id.HasValue)
        {
            result = id.Value;
            return true;
        }
        result = Guid.Empty;
        return false;
    }
}
