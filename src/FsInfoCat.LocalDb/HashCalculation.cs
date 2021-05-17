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
        public HashCalculation()
        {
            Files = new HashSet<FsFile>();
        }

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_LENGTH)]
        [CustomValidation(typeof(Validators), nameof(Validators.IsValidFileLength), ErrorMessage = Constants.ERROR_MESSAGE_FILE_LENGTH)]
        public long Length { get; set; }

        private byte[] _data;

        [CustomValidation(typeof(Validators), nameof(Validators.IsValidMD5Hash), ErrorMessage = Constants.ERROR_MESSAGE_MD5_HASH)]
        [DisplayName(Constants.DISPLAY_NAME_MD5_HASH)]
        public byte[] Data { get => _data; set => _data = (value is null || value.Length == 0) ? null : value; }

        IReadOnlyCollection<byte> IHashCalculation.Data => Data;

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public virtual HashSet<FsFile> Files { get; set; }

        IReadOnlyCollection<IFile> IHashCalculation.Files => Files;

        IReadOnlyCollection<ILocalFile> ILocalHashCalculation.Files => Files;

        public bool TryGetMD5Checksum(out UInt128 result) => UInt128.TryCreate(Data, out result);

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
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
