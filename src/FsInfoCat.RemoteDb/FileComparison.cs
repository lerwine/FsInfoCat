using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FileComparison : IRemoteFileComparison
    {
        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(FileId1), nameof(FileId2));
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
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        // TODO: [AreEqual] bit  NOT NULL,
        [Display(Name = nameof(ModelResources.DisplayName_AreEqual), ResourceType = typeof(ModelResources))]
        public bool AreEqual { get; set; }

        // TODO: [FileId1] uniqueidentifier  NOT NULL,
        public Guid FileId1 { get; set; }

        // TODO: [FileId2] uniqueidentifier  NOT NULL,
        public Guid FileId2 { get; set; }

        // TODO: [CreatedOn] datetime  NOT NULL,
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        // [CreatedById] uniqueidentifier  NOT NULL,
        public Guid CreatedById { get; set; }

        // TODO: [ModifiedOn] datetime  NOT NULL
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        // [ModifiedById] uniqueidentifier  NOT NULL,
        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_File1), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File1Required), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile File1 { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_File2), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File2Required), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile File2 { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IFile IFileComparison.File1 => File1;

        IRemoteFile IRemoteFileComparison.File1 => File1;

        IFile IFileComparison.File2 => File2;

        IRemoteFile IRemoteFileComparison.File2 => File2;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
