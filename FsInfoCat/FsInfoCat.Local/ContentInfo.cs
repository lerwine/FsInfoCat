using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class ContentInfo : NotifyPropertyChanged, ILocalContentInfo
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<MD5Hash?> _hash;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private HashSet<DbFile> _files = new();
        private HashSet<RedundantSet> _redundantSets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required]
        public virtual long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        public virtual MD5Hash? Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

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

        public virtual HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public virtual HashSet<RedundantSet> RedundantSets
        {
            get => _redundantSets;
            set => CheckHashSetChanged(_redundantSets, value, h => _redundantSets = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalContentInfo.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IContentInfo.Files => Files.Cast<IFile>();

        IEnumerable<ILocalRedundantSet> ILocalContentInfo.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<IRedundantSet> IContentInfo.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        #endregion

        public ContentInfo()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _length = AddChangeTracker(nameof(Length), 0L);
            _hash = AddChangeTracker<MD5Hash?>(nameof(Hash), null);
            _upstreamId = AddChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = AddChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = AddChangeTracker(nameof(ModifiedOn), (_createdOn = AddChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalContentInfo entity && Id.Equals(entity.Id));

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnValidate);

        private void OnValidate(EntityEntry<ContentInfo> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            if (Length < 0L)
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, new string[] { nameof(Length) }));
        }

        internal static void BuildEntity(EntityTypeBuilder<ContentInfo> obj)
        {
            obj.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }
    }
}
