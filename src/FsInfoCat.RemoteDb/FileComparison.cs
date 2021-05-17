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
    public class FileComparison : IRemoteFileComparison
    {
        // TODO: Add to interface
        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_1)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_FILE_1)]
        public FsFile File1 { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_2)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_FILE_1)]
        public FsFile File2 { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_ARE_EQUAL)]
        public bool AreEqual { get; set; }

        public Guid FileId1 { get; set; }

        public Guid FileId2 { get; set; }

        public Guid CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public UserProfile CreatedBy { get; set; }

        public UserProfile ModifiedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        IFile IFileComparison.File1 => File1;

        IRemoteFile IRemoteFileComparison.File1 => File1;

        IFile IFileComparison.File2 => File2;

        IRemoteFile IRemoteFileComparison.File2 => File2;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(Id));
            builder.ToTable($"{nameof(FsFile)}{nameof(FileComparison)}1").HasOne(p => p.File1).WithMany(d => d.Comparisons1)
                .HasForeignKey(f => f.FileId1).IsRequired();
            builder.ToTable($"{nameof(FsFile)}{nameof(FileComparison)}2").HasOne(p => p.File2).WithMany(d => d.Comparisons2)
                .HasForeignKey(f => f.FileId2).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedComparisons).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedComparisons).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(File1, new ValidationContext(this, null, null) { MemberName = nameof(File1) }, results);
            Validator.TryValidateProperty(File2, new ValidationContext(this, null, null) { MemberName = nameof(File2) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
