using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    // TODO: Document CrawlJobLogRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class CrawlJobLogRow : LocalDbEntity, ILocalCrawlJobLogRow
    {
        #region Fields

        private Guid? _id;
        private string _rootPath = string.Empty;
        private string _statusMessage = string.Empty;
        private string _statusDetail = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
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

        /// <summary>
        /// Gets root path of the crawl.
        /// </summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootPath), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_rootPath))]
        public string RootPath { get => _rootPath; set => _rootPath = value ?? ""; }

        /// <summary>
        /// Gets a value indicating whether the current crawl configuration has been deactivated.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlStatus StatusCode { get; set; } = CrawlStatus.NotRunning;

        /// <summary>
        /// Gets the date and time when the crawl was started.
        /// </summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime CrawlStart { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets the date and time when the crawl was finshed.
        /// </summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? CrawlEnd { get; set; }

        /// <summary>
        /// Gets the status message.
        /// </summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Message), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_statusMessage))]
        public string StatusMessage { get => _statusMessage; set => _statusMessage = value.AsNonNullTrimmed(); }

        /// <summary>
        /// Gets the status details.
        /// </summary>
        /// <value>The status details.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Details), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_statusDetail))]
        public string StatusDetail { get => _statusDetail; set => _statusDetail = value.AsNonNullTrimmed(); }

        /// <summary>
        /// Gets the maximum recursion depth.
        /// </summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the root <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

        /// <summary>
        /// Gets the maximum total items to crawl.
        /// </summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        public virtual ulong? MaxTotalItems { get; set; }

        /// <summary>
        /// Gets the maximum duration of the crawl.
        /// </summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Range(1, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual long? TTL { get; set; }

        public long FoldersProcessed { get; set; }

        public long FilesProcessed { get; set; }

        public virtual Guid ConfigurationId { get; set; }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalCrawlJobLogRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobLogRow other) => ArePropertiesEqual((ICrawlJobLogRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ICrawlJobLogRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlJobLogRow other) => ArePropertiesEqual((ICrawlJob)other) && RootPath == other.RootPath && StatusCode == other.StatusCode && CrawlEnd == other.CrawlEnd && CreatedOn == other.CreatedOn && ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ICrawlJob" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlJob other) => other is not null && CrawlStart == other.CrawlStart && StatusMessage == other.StatusMessage && StatusDetail == other.StatusDetail && MaxRecursionDepth == other.MaxRecursionDepth &&
            MaxTotalItems == other.MaxTotalItems && TTL == other.TTL && FoldersProcessed == other.FoldersProcessed && FilesProcessed == other.FilesProcessed;

        public abstract bool Equals(ICrawlJob other);

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(_rootPath);
            hash.Add(StatusCode);
            hash.Add(CrawlStart);
            hash.Add(CrawlEnd);
            hash.Add(_statusMessage);
            hash.Add(_statusDetail);
            hash.Add(MaxRecursionDepth);
            hash.Add(MaxTotalItems);
            hash.Add(TTL);
            hash.Add(FoldersProcessed);
            hash.Add(FilesProcessed);
            hash.Add(ConfigurationId);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"RootPath=""{ExtensionMethods.EscapeCsString(_rootPath)}"",
    MaxRecursionDepth={MaxRecursionDepth}, MaxTotalItems={MaxTotalItems}, TTL={TTL},
    StatusCode={StatusCode}, FoldersProcessed={FoldersProcessed}, FilesProcessed={FilesProcessed},
    CrawlStart={CrawlStart:yyyy-mm-ddTHH:mm:ss.fffffff}, CrawlEnd={CrawlEnd:yyyy-mm-ddTHH:mm:ss.fffffff}, StatusMessage=""{ExtensionMethods.EscapeCsString(_statusMessage)}""";

        public override string ToString() => $@"{{ Id={_id}, ConfigurationId={ConfigurationId}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    StatusDetail=""{ExtensionMethods.EscapeCsString(_statusDetail)}"" }}";

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
