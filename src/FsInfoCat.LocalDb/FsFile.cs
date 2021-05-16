using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FsFile : IFile, IValidatableObject
    {
        public FsFile()
        {
            Redundancies = new HashSet<Redundancy>();
            Comparisons1 = new HashSet<Comparison>();
            Comparisons2 = new HashSet<Comparison>();
        }

        public Guid Id { get; set; }

        private string _name = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public FileStatus Status { get; set; }

        public Guid ParentId { get; set; }

        public Guid HashCalculationId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public Guid? FileRelocateTaskId { get; set; }

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_HASH_CALCULATION)]
        [DisplayName(Constants.DISPLAY_NAME_HASH_CALCULATION)]
        public virtual HashCalculation HashCalculation { get; set; }

        IHashCalculation IFile.HashCalculation => HashCalculation;

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_PARENT_DIRECTORY)]
        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public virtual FsDirectory Parent { get; set; }

        ISubDirectory IFile.Parent => Parent;

        public virtual HashSet<Redundancy> Redundancies { get; set; }

        public virtual HashSet<Comparison> Comparisons1 { get; set; }

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => Comparisons1;

        public virtual HashSet<Comparison> Comparisons2 { get; set; }

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => Comparisons2;

        [DisplayName(Constants.DISPLAY_NAME_FILE_RELOCATE_TASK)]
        public virtual FileRelocateTask FileRelocateTask { get; set; }

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(p => p.HashCalculation).WithMany(d => d.Files).HasForeignKey(nameof(HashCalculationId)).IsRequired();
            builder.HasOne(p => p.FileRelocateTask).WithMany(d => d.Files).HasForeignKey(nameof(FileRelocateTaskId));
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
    }
}
