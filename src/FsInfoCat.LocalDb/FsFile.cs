using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.LocalDb
{
    [Table(TABLE_NAME)]
    public class FsFile : ILocalFile, IValidatableObject
    {
        public const string TABLE_NAME = "Files";

        private string _name = "";
        private string _notes = "";

        public FsFile()
        {
            SourceComparisons = new HashSet<FileComparison>();
            TargetComparisons = new HashSet<FileComparison>();
        }

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_FileSystemName).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(p => p.HashInfo).WithMany(d => d.Files).HasForeignKey(nameof(ContentInfoId)).IsRequired();
            //builder.HasOne(d => d.Redundancy).WithOne(r => r.TargetFile).HasForeignKey(nameof(RedundancyId));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            Validator.TryValidateProperty(Parent, new ValidationContext(this, null, null) { MemberName = nameof(Parent) }, results);
            Validator.TryValidateProperty(HashInfo, new ValidationContext(this, null, null) { MemberName = nameof(HashInfo) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_FileSystemName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        [Required]
        public FileCrawlFlags Options { get; set; }

        public Guid ParentId { get; set; }

        public Guid ContentInfoId { get; set; }

        //public Guid? RedundancyId { get; set; }

        public Guid? UpstreamId { get; set; }
        
        [Display(Name = nameof(ModelResources.DisplayName_LastSynchronized), ResourceType = typeof(ModelResources))]
        public DateTime? LastSynchronized { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_LastAccessed), ResourceType = typeof(ModelResources))]
        public DateTime LastAccessed { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LastHashCalculation), ResourceType = typeof(ModelResources))]
        public DateTime? LastHashCalculation { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        public bool Deleted { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public Redundancy Redundancy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_HashCalculation), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_HashCalculationRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual ContentInfo HashInfo { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ParentDirectoryRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FsDirectory Parent { get; set; }

        public virtual HashSet<FileComparison> SourceComparisons { get; set; }

        public virtual HashSet<FileComparison> TargetComparisons { get; set; }

        #endregion

        #region Explicit Members

        IContentInfo IFile.HashInfo => HashInfo;

        ISubDirectory IFile.Parent => Parent;

        IReadOnlyCollection<IFileComparison> IFile.SourceComparisons => SourceComparisons;

        IReadOnlyCollection<IFileComparison> IFile.TargetComparisons => TargetComparisons;

        IReadOnlyCollection<ILocalFileComparison> ILocalFile.Comparisons1 => SourceComparisons;

        IReadOnlyCollection<ILocalFileComparison> ILocalFile.Comparisons2 => TargetComparisons;

        ILocalSubDirectory ILocalFile.Parent => Parent;

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        #endregion
    }
}
