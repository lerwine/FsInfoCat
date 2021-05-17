using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FsFile : ILocalFile, IValidatableObject
    {
        private string _name = "";

        public FsFile()
        {
            Comparisons1 = new HashSet<FileComparison>();
            Comparisons2 = new HashSet<FileComparison>();
        }

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(p => p.HashCalculation).WithMany(d => d.Files).HasForeignKey(nameof(HashCalculationId)).IsRequired();
            builder.HasOne(d => d.Redundancy).WithMany(r => r.Files);
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

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public FileStatus Status { get; set; }

        public Guid ParentId { get; set; }

        public Guid HashCalculationId { get; set; }

        public Guid? RedundancyId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public Redundancy Redundancy { get; set; }

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_HASH_CALCULATION)]
        [DisplayName(Constants.DISPLAY_NAME_HASH_CALCULATION)]
        public virtual HashCalculation HashCalculation { get; set; }

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_PARENT_DIRECTORY)]
        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public virtual FsDirectory Parent { get; set; }

        public virtual HashSet<FileComparison> Comparisons1 { get; set; }

        public virtual HashSet<FileComparison> Comparisons2 { get; set; }

        #endregion

        #region Explicit Members

        IHashCalculation IFile.HashCalculation => HashCalculation;

        ISubDirectory IFile.Parent => Parent;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => Comparisons1;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => Comparisons2;

        IReadOnlyCollection<ILocalFileComparison> ILocalFile.Comparisons1 => Comparisons1;

        IReadOnlyCollection<ILocalFileComparison> ILocalFile.Comparisons2 => Comparisons2;

        ILocalSubDirectory ILocalFile.Parent => Parent;

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        #endregion
    }
}
