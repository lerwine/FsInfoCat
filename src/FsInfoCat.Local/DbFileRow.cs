using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class DbFileRow : LocalDbEntity, ILocalFileRow, ISimpleIdentityReference<DbFileRow>
    {
        #region Fields

        private Guid? _id;
        private string _name = string.Empty;
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

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        [Required]
        public virtual FileCorrelationStatus Status { get; set; } = FileCorrelationStatus.Dissociated;

        [Required]
        public virtual FileCrawlOptions Options { get; set; } = FileCrawlOptions.None;

        [Required]
        public virtual DateTime LastAccessed { get; set; }

        public virtual DateTime? LastHashCalculation { get; set; }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual Guid BinaryPropertySetId { get; set; }

        public virtual Guid? SummaryPropertySetId { get; set; }

        public virtual Guid? DocumentPropertySetId { get; set; }

        public virtual Guid? AudioPropertySetId { get; set; }

        public virtual Guid? DRMPropertySetId { get; set; }

        public virtual Guid? GPSPropertySetId { get; set; }

        public virtual Guid? ImagePropertySetId { get; set; }

        public virtual Guid? MediaPropertySetId { get; set; }

        public virtual Guid? MusicPropertySetId { get; set; }

        public virtual Guid? PhotoPropertySetId { get; set; }

        public virtual Guid? RecordedTVPropertySetId { get; set; }

        public virtual Guid? VideoPropertySetId { get; set; }

        #endregion

        protected DbFileRow()
        {
            CreationTime = LastWriteTime = LastAccessed = CreatedOn;
        }

        DbFileRow IIdentityReference<DbFileRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalFileRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] IFileRow other)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
