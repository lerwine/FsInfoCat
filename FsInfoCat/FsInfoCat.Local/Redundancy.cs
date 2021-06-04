using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class Redundancy : NotifyPropertyChanged, ILocalRedundancy
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _fileId;
        private readonly IPropertyChangeTracker<Guid> _redundantSetId;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<DbFile> _file;
        private readonly IPropertyChangeTracker<RedundantSet> _redundantSet;

        #endregion

        #region Properties

        [Required]
        public virtual Guid FileId
        {
            get => _fileId.GetValue();
            set
            {
                if (_fileId.SetValue(value))
                {
                    DbFile nav = _file.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _file.SetValue(null);
                }
            }
        }

        [Required]
        public virtual Guid RedundantSetId
        {
            get => _redundantSetId.GetValue();
            set
            {
                if (_redundantSetId.SetValue(value))
                {
                    RedundantSet nav = _redundantSet.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _redundantSet.SetValue(null);
                }
            }
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DbFile File
        {
            get => _file.GetValue();
            set
            {
                if (_file.SetValue(value))
                {
                    if (value is null)
                        _fileId.SetValue(Guid.Empty);
                    else
                        _fileId.SetValue(value.Id);
                }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RedundantSet RedundantSet
        {
            get => _redundantSet.GetValue();
            set
            {
                if (_redundantSet.SetValue(value))
                {
                    if (value is null)
                        _redundantSetId.SetValue(Guid.Empty);
                    else
                        _redundantSetId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalRedundancy.File { get => File; set => File = (DbFile)value; }

        IFile IRedundancy.File { get => File; set => File = (DbFile)value; }

        ILocalRedundantSet ILocalRedundancy.RedundantSet { get => RedundantSet; set => RedundantSet = (RedundantSet)value; }

        IRedundantSet IRedundancy.RedundantSet { get => RedundantSet; set => RedundantSet = (RedundantSet)value; }

        #endregion

        public Redundancy()
        {
            _fileId = AddChangeTracker(nameof(FileId), Guid.Empty);
            _redundantSetId = AddChangeTracker(nameof(RedundantSetId), Guid.Empty);
            _reference = AddChangeTracker(nameof(Reference), "", NonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _upstreamId = AddChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = AddChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = AddChangeTracker(nameof(ModifiedOn), (_createdOn = AddChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _file = AddChangeTracker<DbFile>(nameof(File), null);
            _redundantSet = AddChangeTracker<RedundantSet>(nameof(RedundantSet), null);
        }

        public bool IsNew() => !(_fileId.IsSet & _redundantSetId.IsSet);


        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalRedundancy entity && FileId.Equals(entity.FileId) &&
            RedundantSetId.Equals(entity.RedundantSetId));

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(FileId), nameof(RedundantSetId));
            builder.HasOne(sn => sn.File).WithOne(d => d.Redundancy).HasForeignKey<Redundancy>(nameof(FileId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.RedundantSet).WithMany(d => d.Redundancies).HasForeignKey(nameof(RedundantSetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnValidate);

        private void OnValidate(EntityEntry<Redundancy> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            RedundantSet redundantSet = RedundantSet;
            //if (redundantSet is null)
            //    validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired, new string[] { nameof(RedundantSet) }));
            DbFile file = File;
            //if (file is null)
            //    validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired, new string[] { nameof(File) }));
            //else if (!redundantSet.ContentInfoId.Equals(file.ContentId))
            //    validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileInRedundantSet, new string[] { nameof(File) }));
            if (!(redundantSet is null || file is null || redundantSet.ContentInfoId.Equals(file.ContentId)))
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileInRedundantSet, new string[] { nameof(File) }));
        }
    }
}
