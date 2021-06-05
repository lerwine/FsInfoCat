using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class Subdirectory : LocalDbEntity, ILocalSubdirectory
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<DirectoryCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _deleted;
        private readonly IPropertyChangeTracker<Guid?> _parentId;
        private readonly IPropertyChangeTracker<Guid?> _volumeId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<Volume> _volume;
        private HashSet<DbFile> _files = new();
        private HashSet<Subdirectory> _subDirectories = new();
        private HashSet<SubdirectoryAccessError> _accessErrors = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual DirectoryCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<SubdirectoryAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
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

        IEnumerable<IAccessError<ILocalSubdirectory>> ILocalSubdirectory.AccessErrors => AccessErrors.Cast<IAccessError<ILocalSubdirectory>>();

        IEnumerable<IAccessError<ISubdirectory>> ISubdirectory.AccessErrors => AccessErrors.Cast<IAccessError<ISubdirectory>>();

        #endregion

        public Subdirectory()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = AddChangeTracker(nameof(Options), DirectoryCrawlOptions.None);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _deleted = AddChangeTracker(nameof(Deleted), false);
            _parentId = AddChangeTracker<Guid?>(nameof(ParentId), null);
            _volumeId = AddChangeTracker<Guid?>(nameof(VolumeId), null);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _volume = AddChangeTracker<Volume>(nameof(Volume), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<Subdirectory> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Volume).WithOne(d => d.RootDirectory).HasForeignKey<Subdirectory>(nameof(VolumeId)).OnDelete(DeleteBehavior.Restrict);//.HasPrincipalKey<Volume>(nameof(Local.Volume.Id));
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateOptions(results);
                ValidateParentAndVolume(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(Options):
                        ValidateOptions(results);
                        break;
                    case nameof(Parent):
                    case nameof(Volume):
                    case nameof(Name):
                        ValidateParentAndVolume(validationContext, results);
                        break;
                }
        }

        private void ValidateParentAndVolume(ValidationContext validationContext, List<ValidationResult> results)
        {
            Subdirectory parent = Parent;
            Volume volume = Volume;
            Guid id = Id;
            LocalDbContext dbContext;
            if (parent is null)
            {
                if (volume is null)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, new string[] { nameof(Parent) }));
                else if ((dbContext = validationContext.GetService<LocalDbContext>()) is not null)
                {
                    Guid volumeId = volume.Id;
                    var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.VolumeId == volumeId select sn;
                    if (entities.Any())
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, new string[] { nameof(Volume) }));
                }
            }
            else if (volume is null)
            {
                if (Id.Equals(parent.Id))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                else if ((dbContext = validationContext.GetService<LocalDbContext>()) is not null)
                {
                    string name = Name;
                    if (string.IsNullOrEmpty(name))
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, new string[] { nameof(Name) }));
                    else
                    {
                        Guid parentId = parent.Id;
                        var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.ParentId == parentId && sn.Name == name select sn;
                        if (entities.Any())
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
                        else
                            while (parent.ParentId.HasValue)
                            {
                                if (parent.ParentId.Value.Equals(Id))
                                {
                                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                                    break;
                                }
                                if ((parent = dbContext.Subdirectories.Find(parent.ParentId)) is null)
                                    break;
                            }
                    }
                }
            }
            else
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeAndParent, new string[] { nameof(Volume) }));
        }

        private void ValidateOptions(List<ValidationResult> results)
        {
            if (!Enum.IsDefined(Options))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, new string[] { nameof(Options) }));
        }
    }
}
