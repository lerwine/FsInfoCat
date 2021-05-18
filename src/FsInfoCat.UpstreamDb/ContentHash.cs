using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class ContentHash : IUpstreamContentHash
    {
        private byte[] _data;

        internal static void BuildEntity(EntityTypeBuilder<ContentHash> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Data)).HasMaxLength(UInt128.ByteSize).IsFixedLength();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHashCalculations).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHashCalculations).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Length, new ValidationContext(this, null, null) { MemberName = nameof(Length) }, results);
            Validator.TryValidateProperty(Data, new ValidationContext(this, null, null) { MemberName = nameof(Data) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        public ContentHash()
        {
            Files = new HashSet<FsFile>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_MD5Hash), ResourceType = typeof(ModelResources))]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidMD5Hash), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MD5HashLength), ErrorMessageResourceType = typeof(ModelResources))]
        public byte[] Data { get => _data; set => _data = (value is null || value.Length == 0) ? null : value; }

        [Display(Name = nameof(ModelResources.DisplayName_FileLength), ResourceType = typeof(ModelResources))]
        [Required]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidFileLength), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileLengthNegative), ErrorMessageResourceType = typeof(ModelResources))]
        public long Length { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<FsFile> Files { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IFile> IContentHash.Files => Files;

        IReadOnlyCollection<IUpstreamFile> IUpstreamContentHash.Files => Files;

        IReadOnlyCollection<byte> IContentHash.Data => Data;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
