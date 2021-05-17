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
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
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

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public FileStatus Status { get; set; }

        public Guid? RedundancyId { get; set; }

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

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_HASH_CALCULATION)]
        [DisplayName(Constants.DISPLAY_NAME_HASH_CALCULATION)]
        public HashCalculation HashCalculation { get; set; }

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_PARENT_DIRECTORY)]
        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public FsDirectory Parent { get; set; }

        public Redundancy Redundancy { get; set; }

        public HashSet<FileComparison> Comparisons1 { get; set; }

        public HashSet<FileComparison> Comparisons2 { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_RELOCATE_TASK)]
        public virtual FileRelocateTask FileRelocateTask { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
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
