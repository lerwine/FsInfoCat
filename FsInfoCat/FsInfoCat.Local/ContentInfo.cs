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
    public class ContentInfo : NotifyPropertyChanged, ILocalContentInfo
    {
        /*
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Length"	BIGINT NOT NULL CHECK(Length>0),
	"Hash"	BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_ContentInfo" PRIMARY KEY("Id"),
	CONSTRAINT "UK_LengthHash" UNIQUE("Length","Hash"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
         */
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<byte[]> _hash;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private HashSet<DbFile> _files = new();
        private HashSet<RedundantSet> _redundantSets = new();

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Required]
        public virtual long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual byte[] Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

        /// <summary></summary>
        /// <remarks>UNIQUEIDENTIFIER DEFAULT NULL</remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
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
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _length = CreateChangeTracker(nameof(Length), 0L);
            _hash = CreateChangeTracker(nameof(Hash), null, NotEmptyOrNullValueArrayCoersion<byte>.Default);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalContentInfo entity && Id.Equals(entity.Id));

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnValidate);

        private void OnValidate(EntityEntry<ContentInfo> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }
    }
}
