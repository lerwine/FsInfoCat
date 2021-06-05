using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class DbFile : LocalDbEntity, ILocalFile
    {
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
        private readonly IPropertyChangeTracker<Guid?> _extendedPropertiesId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<ContentInfo> _content;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private readonly IPropertyChangeTracker<ExtendedProperties> _extendedProperties;
        private HashSet<FileAccessError> _accessErrors = new();
        private HashSet<FileComparison> _comparisonSources = new();
        private HashSet<FileComparison> _comparisonTargets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

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

        public virtual Guid? ExtendedPropertiesId
        {
            get => _extendedPropertiesId.GetValue();
            set
            {
                if (_extendedPropertiesId.SetValue(value))
                {
                    ExtendedProperties nav = _extendedProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ExtendedProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ExtendedProperties ExtendedProperties
        {
            get => _extendedProperties.GetValue();
            set
            {
                if (_extendedProperties.SetValue(value))
                {
                    if (value is null)
                        _extendedPropertiesId.SetValue(null);
                    else
                        _extendedPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonSources), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> ComparisonSources
        {
            get => _comparisonSources;
            set => CheckHashSetChanged(_comparisonSources, value, h => _comparisonSources = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonTargets), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

        ILocalExtendedProperties ILocalFile.ExtendedProperties { get => ExtendedProperties; set => ExtendedProperties = (ExtendedProperties)value; }

        IExtendedProperties IFile.ExtendedProperties { get => ExtendedProperties; set => ExtendedProperties = (ExtendedProperties)value; }

        IEnumerable<IAccessError<ILocalFile>> ILocalFile.AccessErrors => AccessErrors.Cast<IAccessError<ILocalFile>>();

        IEnumerable<IAccessError<IFile>> IFile.AccessErrors => AccessErrors.Cast<IAccessError<IFile>>();

        #endregion

        public DbFile()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = AddChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastHashCalculation = AddChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _deleted = AddChangeTracker(nameof(Deleted), false);
            _parentId = AddChangeTracker(nameof(ParentId), Guid.Empty);
            _contentId = AddChangeTracker(nameof(ContentId), Guid.Empty);
            _extendedPropertiesId = AddChangeTracker<Guid?>(nameof(UpstreamId), null);
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _content = AddChangeTracker<ContentInfo>(nameof(Content), null);
            _redundancy = AddChangeTracker<Redundancy>(nameof(Redundancy), null);
            _extendedProperties = AddChangeTracker<ExtendedProperties>(nameof(ExtendedProperties), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Content).WithMany(d => d.Files).HasForeignKey(nameof(ContentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.ExtendedProperties).WithMany(d => d.Files).HasForeignKey(nameof(ExtendedPropertiesId)).OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            // TODO: Implement OnValidate(ValidationContext, List{ValidationResult})
            base.OnValidate(validationContext, results);
        }

        private void OnValidate(EntityEntry<DbFile> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            string name = Name;
            if (string.IsNullOrEmpty(name))
                return;
            Guid id = Id;
            Guid parentId = ParentId;
            // TODO: Need to test whether this fails to skip an item where the id matches
            if (dbContext.Files.Any(sn => sn.ParentId == parentId && sn.Name == name && id != sn.Id))
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
        }
    }
}
