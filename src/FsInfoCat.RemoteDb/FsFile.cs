using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FsFile : IRemoteFile
    {
        private string _name = "";

        public FsFile()
        {
            Comparisons1 = new HashSet<FileComparison>();
            Comparisons2 = new HashSet<FileComparison>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            Validator.TryValidateProperty(Parent, new ValidationContext(this, null, null) { MemberName = nameof(Parent) }, results);
            Validator.TryValidateProperty(HashCalculation, new ValidationContext(this, null, null) { MemberName = nameof(HashCalculation) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DBSettings.Default.DbColMaxLen_FileSystemName).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(p => p.HashCalculation).WithMany(d => d.Files).HasForeignKey(nameof(HashCalculationId)).IsRequired();
            builder.HasOne(d => d.Redundancy).WithMany(r => r.Files);
            builder.HasOne(p => p.FileRelocateTask).WithMany(d => d.Files).HasForeignKey(nameof(FileRelocateTaskId));
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedFiles).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedFiles).IsRequired();
        }

        #region Column Properties

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public Guid HashCalculationId { get; set; }

        public Guid? FileRelocateTaskId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_FileSystemName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        public FileStatus Status { get; set; }

        public Guid? RedundancyId { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_HashCalculation), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_HashCalculationRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public HashCalculation HashCalculation { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ParentDirectoryRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public FsDirectory Parent { get; set; }

        public Redundancy Redundancy { get; set; }

        public HashSet<FileComparison> Comparisons1 { get; set; }

        public HashSet<FileComparison> Comparisons2 { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_FileRelocationTask), ResourceType = typeof(ModelResources))]
        public virtual FileRelocateTask FileRelocateTask { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IRemoteFileComparison> IRemoteFile.Comparisons1 => Comparisons1;

        IReadOnlyCollection<IRemoteFileComparison> IRemoteFile.Comparisons2 => Comparisons2;

        IRemoteSubDirectory IRemoteFile.Parent => Parent;

        IHashCalculation IFile.HashCalculation => HashCalculation;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => Comparisons1;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => Comparisons2;

        ISubDirectory IFile.Parent => Parent;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        IRemoteHashCalculation IRemoteFile.HashCalculation => HashCalculation;

        IRemoteRedundancy IRemoteFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        #endregion
    }
}
