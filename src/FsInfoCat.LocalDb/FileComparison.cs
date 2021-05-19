using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FileComparison : ILocalFileComparison
    {
        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(SourceFileId), nameof(TargetFileId));
            //builder.ToTable($"{nameof(FileComparison)}{nameof(File1)}").HasOne(p => p.File1).WithMany(d => d.Comparisons1)
            //    .HasForeignKey(f => f.FileId1).IsRequired();
            //builder.ToTable($"{nameof(FileComparison)}{nameof(File1)}").HasOne(p => p.File2).WithMany(d => d.Comparisons2)
            //    .HasForeignKey(f => f.FileId2).IsRequired();
            builder.HasOne(p => p.SourceFile).WithMany(d => d.SourceComparisons).HasForeignKey(nameof(SourceFileId)).IsRequired();
            builder.HasOne(p => p.File2).WithMany(d => d.TargetComparisons).HasForeignKey(nameof(TargetFileId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(SourceFile, new ValidationContext(this, null, null) { MemberName = nameof(SourceFile) }, results);
            Validator.TryValidateProperty(File2, new ValidationContext(this, null, null) { MemberName = nameof(File2) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid SourceFileId { get; set; }

        public Guid TargetFileId { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_AreEqual), ResourceType = typeof(ModelResources))]
        public bool AreEqual { get; set; }

        public Guid? UpstreamId { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LastSynchronized), ResourceType = typeof(ModelResources))]
        public DateTime? LastSynchronized { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_File1), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File1Required), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FsFile SourceFile { get; set; }

        // TODO: Rename to TargetFile
        [Display(Name = nameof(ModelResources.DisplayName_File2), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_File2Required), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FsFile File2 { get; set; }

        #endregion

        #region Explicit Members

        IFile IFileComparison.SourceFile => SourceFile;

        IFile IFileComparison.TargetFile => File2;

        ILocalFile ILocalFileComparison.SourceFile => SourceFile;

        ILocalFile ILocalFileComparison.TargetFile => File2;

        #endregion
    }
}
