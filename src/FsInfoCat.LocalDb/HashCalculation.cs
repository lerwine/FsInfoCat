using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class HashCalculation : ILocalHashCalculation
    {
        private byte[] _data;

        public HashCalculation()
        {
            Files = new HashSet<FsFile>();
        }

        internal static void BuildEntity(EntityTypeBuilder<HashCalculation> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Data)).HasMaxLength(UInt128.ByteSize).IsFixedLength();
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

        public bool TryGetMD5Checksum(out UInt128 result) => UInt128.TryCreate(Data, out result);

        #region Column Properties

        public Guid Id { get; set; }


        [Display(Name = nameof(ModelResources.DisplayName_FileLength), ResourceType = typeof(ModelResources))]
        [Required]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidFileLength), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileLengthNegative), ErrorMessageResourceType = typeof(ModelResources))]
        public long Length { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_MD5Hash), ResourceType = typeof(ModelResources))]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidMD5Hash), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MD5HashLength), ErrorMessageResourceType = typeof(ModelResources))]
        public byte[] Data { get => _data; set => _data = (value is null || value.Length == 0) ? null : value; }

        [Required()]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        [Required()]
        public DateTime ModifiedOn { get; set; }

        public Guid? UpstreamId { get; set; }

        public DateTime? LastSynchronized { get; set; }

        #endregion

        #region Navigation Properties

        public virtual HashSet<FsFile> Files { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<byte> IHashCalculation.Data => Data;

        IReadOnlyCollection<IFile> IHashCalculation.Files => Files;

        IReadOnlyCollection<ILocalFile> ILocalHashCalculation.Files => Files;

        #endregion
    }
}
