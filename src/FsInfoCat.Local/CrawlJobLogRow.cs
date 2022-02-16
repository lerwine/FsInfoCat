using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
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

        /// <summary>Gets root path of the crawl.</summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootPath), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string RootPath { get => _rootPath; set => _rootPath = value ?? ""; }

        /// <summary>Gets a value indicating whether the current crawl configuration has been deactivated.</summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlStatus StatusCode { get; set; } = CrawlStatus.NotRunning;

        /// <summary>Gets the date and time when the crawl was started.</summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime CrawlStart { get; set; } = DateTime.Now;

        /// <summary>Gets the date and time when the crawl was finshed.</summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? CrawlEnd { get; set; }

        /// <summary>Gets the status message.</summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Message), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string StatusMessage { get => _statusMessage; set => _statusMessage = value.AsNonNullTrimmed(); }

        /// <summary>Gets the status details.</summary>
        /// <value>The status details.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Details), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string StatusDetail { get => _statusDetail; set => _statusDetail = value.AsNonNullTrimmed(); }

        /// <summary>Gets the maximum recursion depth.</summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the <see cref="Root" /> <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

        /// <summary>Gets the maximum total items to crawl.</summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        public virtual ulong? MaxTotalItems { get; set; }

        /// <summary>Gets the maximum duration of the crawl.</summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Range(1, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual long? TTL { get; set; }

        public long FoldersProcessed { get; set; }

        public long FilesProcessed { get; set; }

        public virtual Guid ConfigurationId { get; set; }

        #endregion

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobLogRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlJobLogRow other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(ICrawlJob other);
    }
}
