using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class HashCalculation : IRemoteHashCalculation
    {
        public HashCalculation()
        {
            Files = new HashSet<FsFile>();
        }

        internal static void BuildEntity(EntityTypeBuilder<HashCalculation> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Data)).HasMaxLength(UInt128.ByteSize).IsFixedLength();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHashCalculations).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHashCalculations).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Length, new ValidationContext(this, null, null) { MemberName = nameof(Length) }, results);
            Validator.TryValidateProperty(Data, new ValidationContext(this, null, null) { MemberName = nameof(Data) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        public bool TryGetMD5Checksum(out UInt128 result) => UInt128.TryCreate(Data, out result);

        #region Column Properties

        [CustomValidation(typeof(Validators), nameof(Validators.IsValidMD5Hash), ErrorMessage = Constants.ERROR_MESSAGE_MD5_HASH)]
        [DisplayName(Constants.DISPLAY_NAME_MD5_HASH)]
        public byte[] Data { get; set; }

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_LENGTH)]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidFileLength), ErrorMessage = Constants.ERROR_MESSAGE_FILE_LENGTH)]
        public long Length { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        [Required]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        [Required]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<FsFile> Files { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IFile> IHashCalculation.Files => Files;

        IReadOnlyCollection<IRemoteFile> IRemoteHashCalculation.Files => Files;

        IReadOnlyCollection<byte> IHashCalculation.Data => Data;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
