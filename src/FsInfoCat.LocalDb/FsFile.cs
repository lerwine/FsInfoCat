using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
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
            builder.Property(nameof(Name)).HasMaxLength(DBSettings.Default.DbColMaxLen_FileSystemName).IsRequired();
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
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        // TODO: [Id] uniqueidentifier  NOT NULL,
        public Guid Id { get; set; }

        // [Name] nvarchar(128)  NOT NULL,
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_FileSystemName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        // TODO: [Status] tinyint  NOT NULL,
        public FileStatus Status { get; set; }

        // [ParentId] uniqueidentifier  NOT NULL,
        public Guid ParentId { get; set; }

        // [HashCalculationId] uniqueidentifier  NOT NULL,
        public Guid HashCalculationId { get; set; }

        public Guid? RedundancyId { get; set; }

        public Guid? UpstreamId { get; set; }

        public DateTime? LastSynchronized { get; set; }

        // TODO: [CreatedOn] datetime  NOT NULL,
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        // TODO: [ModifiedOn] datetime  NOT NULL
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public Redundancy Redundancy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_HashCalculation), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_HashCalculationRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual HashCalculation HashCalculation { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ParentDirectoryRequired), ErrorMessageResourceType = typeof(ModelResources))]
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
