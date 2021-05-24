using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class DbFile : NotifyPropertyChanged, ILocalFile
    {
        /*
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<15) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "LastHashCalculation" DATETIME DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Deleted" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ContentInfoId"	UNIQUEIDENTIFIER NOT NULL,
	"ParentId"	UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
	CONSTRAINT "FK_FileSubdirectory" FOREIGN KEY("ParentId") REFERENCES "Subdirectories"("Id"),
	CONSTRAINT "FK_FileContentInfo" FOREIGN KEY("ContentInfoId") REFERENCES "ContentInfos"("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
         */
        #region Fields

        public const string TABLE_NAME = "Files";

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<FileCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<DateTime?> _lastHashCalculation;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _deleted;
        private readonly IPropertyChangeTracker<Guid> _parentId;
        private readonly IPropertyChangeTracker<Guid> _contentId;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<ContentInfo> _content;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private HashSet<FileComparison> _comparisonSources = new();
        private HashSet<FileComparison> _comparisonTargets = new();

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        /// <remarks>
        /// UNIQUEIDENTIFIER NOT NULL
        /// </remarks>
        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        /// <remarks>TEXT NOT NULL DEFAULT ''</remarks>
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

        /// <remarks>UNIQUEIDENTIFIER NOT NULL</remarks>
        public virtual Guid ParentId
        {
            get => _parentId.GetValue();
            set
            {
                if (_parentId.SetValue(value))
                {
                    Subdirectory nav = _parent.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _parent.SetValue(null);
                }
            }
        }

        /// <remarks>UNIQUEIDENTIFIER NOT NULL</remarks>
        public virtual Guid ContentId
        {
            get => _contentId.GetValue();
            set
            {
                if (_contentId.SetValue(value))
                {
                    ContentInfo nav = _content.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _content.SetValue(null);
                }
            }
        }

        /// <remarks>UNIQUEIDENTIFIER DEFAULT NULL</remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// DATETIME NOT NULL DEFAULT (datetime('now','localtime'))
        /// </remarks>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// DATETIME NOT NULL DEFAULT (datetime('now','localtime'))
        /// </remarks>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        public virtual ContentInfo Content
        {
            get => _content.GetValue();
            set
            {
                if (_content.SetValue(value))
                {
                    if (value is null)
                        _contentId.SetValue(Guid.Empty);
                    else
                        _contentId.SetValue(value.Id);
                }
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                {
                    if (value is null)
                        _parentId.SetValue(Guid.Empty);
                    else
                        _parentId.SetValue(value.Id);
                }
            }
        }

        public virtual Redundancy Redundancy { get => _redundancy.GetValue(); set => _redundancy.SetValue(value); }

        public virtual HashSet<FileComparison> ComparisonSources
        {
            get => _comparisonSources;
            set => CheckHashSetChanged(_comparisonSources, value, h => _comparisonSources = h);
        }

        public virtual HashSet<FileComparison> ComparisonTargets
        {
            get => _comparisonTargets;
            set => CheckHashSetChanged(_comparisonTargets, value, h => _comparisonTargets = h);
        }

        #endregion

        #region Explicit Members

        ILocalContentInfo ILocalFile.Content { get => Content; set => Content = (ContentInfo)value; }

        IContentInfo IFile.Content { get => Content; set => Content = (ContentInfo)value; }

        ILocalSubdirectory ILocalFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory IFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        IEnumerable<ILocalComparison> ILocalFile.ComparisonSources => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonSources => ComparisonSources.Cast<IComparison>();

        IEnumerable<ILocalComparison> ILocalFile.ComparisonTargets => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonTargets => ComparisonSources.Cast<IComparison>();

        #endregion

        public DbFile()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _name = CreateChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = CreateChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastHashCalculation = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _deleted = CreateChangeTracker(nameof(Deleted), false);
            _parentId = CreateChangeTracker(nameof(ParentId), Guid.Empty);
            _contentId = CreateChangeTracker(nameof(ContentId), Guid.Empty);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _lastAccessed = CreateChangeTracker(nameof(LastAccessed), _createdOn.GetValue());
            _parent = CreateChangeTracker<Subdirectory>(nameof(Parent), null);
            _content = CreateChangeTracker<ContentInfo>(nameof(Content), null);
            _redundancy = CreateChangeTracker<Redundancy>(nameof(Redundancy), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalFile entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(sn => sn.Content).WithMany(d => d.Files).HasForeignKey(nameof(ContentId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnBeforeValidate, OnValidate);

        private void OnBeforeValidate(EntityEntry<DbFile> entityEntry, LocalDbContext dbContext)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }

        private void OnValidate(EntityEntry<DbFile> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }
    }
}
