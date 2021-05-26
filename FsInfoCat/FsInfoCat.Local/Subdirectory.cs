using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class Subdirectory : NotifyPropertyChanged, ILocalSubdirectory
    {
        /*
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<64) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Deleted" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ParentId"	UNIQUEIDENTIFIER,
	"VolumeId"	UNIQUEIDENTIFIER,
	CONSTRAINT "PK_Subdirectoriess" PRIMARY KEY("Id"),
	CONSTRAINT "FK_SubdirectoryParent" FOREIGN KEY("ParentId") REFERENCES "Subdirectories"("Id"),
	CONSTRAINT "FK_SubdirectoryVolume" FOREIGN KEY("VolumeId") REFERENCES "Volumes"("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND
        ((ParentId IS NULL AND VolumeId IS NOT NULL) OR
        (ParentId IS NOT NULL AND VolumeId IS NULL AND length(trim(Name))>0)))
         */
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<DirectoryCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _deleted;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<Guid?> _parentId;
        private readonly IPropertyChangeTracker<Guid?> _volumeId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<Volume> _volume;
        private HashSet<DbFile> _files = new ();
        private HashSet<Subdirectory> _subDirectories = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual DirectoryCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        /// <remarks>TEXT NOT NULL DEFAULT ''</remarks>
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

        /// <remarks>UNIQUEIDENTIFIER</remarks>
        public virtual Guid? ParentId
        {
            get => _parentId.GetValue();
            set
            {
                if (_parentId.SetValue(value))
                {
                    Subdirectory nav = _parent.GetValue();
                    if (!(nav is null || (value.HasValue && nav.Id.Equals(value.Value))))
                        _parent.SetValue(null);
                }
            }
        }

        /// <remarks>UNIQUEIDENTIFIER</remarks>
        public virtual Guid? VolumeId
        {
            get => _volumeId.GetValue();
            set
            {
                if (_volumeId.SetValue(value))
                {
                    Volume nav = _volume.GetValue();
                    if (!(nav is null || (value.HasValue && nav.Id.Equals(value.Value))))
                        _volume.SetValue(null);
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

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                {
                    if (value is null)
                        _parentId.SetValue(null);
                    else
                        _parentId.SetValue(value.Id);
                }
            }
        }

        public virtual Volume Volume
        {
            get => _volume.GetValue();
            set
            {
                if (_volume.SetValue(value))
                {
                    if (value is null)
                        _volumeId.SetValue(null);
                    else
                        _volumeId.SetValue(value.Id);
                }
            }
        }

        public virtual HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public virtual HashSet<Subdirectory> SubDirectories
        {
            get => _subDirectories;
            set => CheckHashSetChanged(_subDirectories, value, h => _subDirectories = h);
        }

        #endregion

        #region Explicit Members

        ILocalSubdirectory ILocalSubdirectory.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory ISubdirectory.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalVolume ILocalSubdirectory.Volume { get => Volume; set => Volume = (Volume)value; }

        IVolume ISubdirectory.Volume { get => Volume; set => Volume = (Volume)value; }

        IEnumerable<ILocalFile> ILocalSubdirectory.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> ISubdirectory.Files => Files.Cast<IFile>();

        IEnumerable<ILocalSubdirectory> ILocalSubdirectory.SubDirectories => SubDirectories.Cast<ILocalSubdirectory>();

        IEnumerable<ISubdirectory> ISubdirectory.SubDirectories => SubDirectories.Cast<ISubdirectory>();

        #endregion

        public Subdirectory()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _name = CreateChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = CreateChangeTracker(nameof(DirectoryCrawlOptions), DirectoryCrawlOptions.None);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _deleted = CreateChangeTracker(nameof(Deleted), false);
            _parentId = CreateChangeTracker<Guid?>(nameof(ParentId), null);
            _volumeId = CreateChangeTracker<Guid?>(nameof(VolumeId), null);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _lastAccessed = CreateChangeTracker(nameof(LastAccessed), _createdOn.GetValue());
            _parent = CreateChangeTracker<Subdirectory>(nameof(Parent), null);
            _volume = CreateChangeTracker<Volume>(nameof(Volume), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalSubdirectory entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<Subdirectory> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
            builder.HasOne(sn => sn.Volume).WithOne(d => d.RootDirectory).HasForeignKey<Subdirectory>(nameof(VolumeId));//.HasPrincipalKey<Volume>(nameof(Local.Volume.Id));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnValidate);

        private void OnValidate(EntityEntry<Subdirectory> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }
    }
}
