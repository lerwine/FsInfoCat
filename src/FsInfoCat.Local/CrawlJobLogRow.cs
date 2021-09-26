using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class CrawlJobLogRow : LocalDbEntity, ILocalCrawlJobLogRow
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _rootPath;
        private readonly IPropertyChangeTracker<CrawlStatus> _statusCode;
        private readonly IPropertyChangeTracker<DateTime> _crawlStart;
        private readonly IPropertyChangeTracker<DateTime?> _crawlEnd;
        private readonly IPropertyChangeTracker<string> _statusMessage;
        private readonly IPropertyChangeTracker<string> _statusDetail;
        private readonly IPropertyChangeTracker<ushort> _maxRecursionDepth;
        private readonly IPropertyChangeTracker<ulong?> _totalMaxItems;
        private readonly IPropertyChangeTracker<long?> _ttl;
        private readonly IPropertyChangeTracker<long> _foldersProcessed;
        private readonly IPropertyChangeTracker<long> _filesProcessed;
        private readonly IPropertyChangeTracker<Guid> _configurationId;

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
            get => _id.GetValue();
            set
            {
                if (_id.IsSet)
                {
                    Guid id = _id.GetValue();
                    if (id.Equals(value))
                        return;
                    if (!id.Equals(Guid.Empty))
                        throw new InvalidOperationException();
                }
                _id.SetValue(value);
            }
        }

        /// <summary>Gets root path of the crawl.</summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootPath), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string RootPath { get => _rootPath.GetValue(); set => _rootPath.SetValue(value); }

        /// <summary>Gets a value indicating whether the current crawl configuration has been deactivated.</summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlStatus StatusCode { get => _statusCode.GetValue(); set => _statusCode.SetValue(value); }

        /// <summary>Gets the date and time when the crawl was started.</summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime CrawlStart { get => _crawlStart.GetValue(); set => _crawlStart.SetValue(value); }

        /// <summary>Gets the date and time when the crawl was finshed.</summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DateTime? CrawlEnd { get => _crawlEnd.GetValue(); set => _crawlEnd.SetValue(value); }

        /// <summary>Gets the status message.</summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Message), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string StatusMessage { get => _statusMessage.GetValue(); set => _statusMessage.SetValue(value); }

        /// <summary>Gets the status details.</summary>
        /// <value>The status details.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Details), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public string StatusDetail { get => _statusDetail.GetValue(); set => _statusDetail.SetValue(value); }

        /// <summary>Gets the maximum recursion depth.</summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the <see cref="Root" /> <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ushort MaxRecursionDepth { get => _maxRecursionDepth.GetValue(); set => _maxRecursionDepth.SetValue(value); }

        /// <summary>Gets the maximum total items to crawl.</summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        public virtual ulong? MaxTotalItems { get => _totalMaxItems.GetValue(); set => _totalMaxItems.SetValue(value); }

        /// <summary>Gets the maximum duration of the crawl.</summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Range(1, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual long? TTL { get => _ttl.GetValue(); set => _ttl.SetValue(value); }

        public long FoldersProcessed { get => _foldersProcessed.GetValue(); set => _foldersProcessed.SetValue(value); }

        public long FilesProcessed { get => _filesProcessed.GetValue(); set => _filesProcessed.SetValue(value); }

        public virtual Guid ConfigurationId
        {
            get => _configurationId.GetValue();
            set
            {
                if (_configurationId.SetValue(value))
                    OnConfigurationIdChanged(value);
                //{
                //    CrawlConfiguration nav = _configuration.GetValue();
                //    if (!(nav is null || nav.Id.Equals(value)))
                //        _configuration.SetValue(null);
                //}
            }
        }

        #endregion

        public CrawlJobLogRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _rootPath = AddChangeTracker(nameof(RootPath), "", TrimmedNonNullStringCoersion.Default);
            _statusCode = AddChangeTracker(nameof(StatusCode), CrawlStatus.NotRunning);
            _crawlStart = AddChangeTracker(nameof(CrawlStart), DateTime.Now);
            _crawlEnd = AddChangeTracker<DateTime?>(nameof(CrawlEnd), null);
            _statusMessage = AddChangeTracker(nameof(StatusMessage), "", TrimmedNonNullStringCoersion.Default);
            _statusDetail = AddChangeTracker(nameof(StatusDetail), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _maxRecursionDepth = AddChangeTracker(nameof(MaxRecursionDepth), DbConstants.DbColDefaultValue_MaxRecursionDepth);
            _totalMaxItems = AddChangeTracker<ulong?>(nameof(MaxTotalItems), null);
            _ttl = AddChangeTracker<long?>(nameof(TTL), null);
            _foldersProcessed = AddChangeTracker(nameof(FoldersProcessed), 0L);
            _filesProcessed = AddChangeTracker(nameof(FilesProcessed), 0L);
            _configurationId = AddChangeTracker(nameof(ConfigurationId), Guid.Empty);
        }

        protected virtual void OnConfigurationIdChanged(Guid value) { }

    }
}
