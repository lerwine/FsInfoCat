using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class FileComparison : IUpstreamFileComparison
    {
        
        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(SourceFileId), nameof(TargetFileId));
            //builder.ToTable($"{nameof(FileComparison)}{nameof(File1)}").HasOne(p => p.File1).WithMany(d => d.Comparisons1)
            //    .HasForeignKey(f => f.FileId1).IsRequired();
            //builder.ToTable($"{nameof(FileComparison)}{nameof(File1)}").HasOne(p => p.File2).WithMany(d => d.Comparisons2)
            //    .HasForeignKey(f => f.FileId2).IsRequired();
            builder.HasOne(p => p.SourceFile).WithMany(d => d.SourceComparisons).HasForeignKey(nameof(SourceFileId)).IsRequired();
            builder.HasOne(p => p.TargetFile).WithMany(d => d.TargetComparisons).HasForeignKey(nameof(TargetFileId)).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedComparisons).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedComparisons).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(SourceFile, new ValidationContext(this, null, null) { MemberName = nameof(SourceFile) }, results);
            Validator.TryValidateProperty(TargetFile, new ValidationContext(this, null, null) { MemberName = nameof(TargetFile) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        [Display(Name = nameof(ModelResources.DisplayName_AreEqual), ResourceType = typeof(ModelResources))]
        public bool AreEqual { get; set; }

        // TODO: Rename to SourceFileId
        public Guid SourceFileId { get; set; }

        // TODO: Rename to TargetFileId
        public Guid TargetFileId { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_File1), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File1Required), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile SourceFile { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_File2), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File2Required), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile TargetFile { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IFile IFileComparison.SourceFile => SourceFile;

        IUpstreamFile IUpstreamFileComparison.SourceFile => SourceFile;

        IFile IFileComparison.TargetFile => TargetFile;

        IUpstreamFile IUpstreamFileComparison.TargetFile => TargetFile;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
