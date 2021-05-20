using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.UpstreamDb
{
    [Table(DbConstants.TableName_FsFile)]
    public class FsFile : IUpstreamFile
    {
        private string _name = "";
        private string _notes = "";

        public FsFile()
        {
            SourceComparisons = new HashSet<FileComparison>();
            TargetComparisons = new HashSet<FileComparison>();
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

        internal static void BuildEntity(EntityTypeBuilder<FsFile> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_FileSystemName).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired();
            builder.HasOne(p => p.HashInfo).WithMany(d => d.Files).HasForeignKey(nameof(ContentInfoId)).IsRequired();
            builder.HasOne(p => p.FileRelocateTask).WithMany(d => d.Files).HasForeignKey(nameof(FileRelocateTaskId));
            //builder.HasOne(d => d.Redundancy).WithOne(r => r.TargetFile).HasForeignKey(nameof(RedundancyId));
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedFiles).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedFiles).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        #region Column Properties

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public Guid ContentInfoId { get; set; }

        //public Guid? RedundancyId { get; set; }

        public Guid? FileRelocateTaskId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_FileSystemName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        [Required]
        public FileCrawlFlags Options { get; set; }

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
        public Guid CreatedById { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        [Required]
        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_HashCalculation), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_HashCalculationRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public ContentInfo HashInfo { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ParentDirectoryRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public FsDirectory Parent { get; set; }

        public Redundancy Redundancy { get; set; }

        public HashSet<FileComparison> SourceComparisons { get; set; }

        public HashSet<FileComparison> TargetComparisons { get; set; }

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

        IReadOnlyCollection<IUpstreamFileComparison> IUpstreamFile.SourceComparisons => SourceComparisons;

        IReadOnlyCollection<IUpstreamFileComparison> IUpstreamFile.TargetComparisons => TargetComparisons;

        IUpstreamSubDirectory IUpstreamFile.Parent => Parent;

        IContentInfo IFile.HashInfo => HashInfo;

        IReadOnlyCollection<IFileComparison> IFile.SourceComparisons => SourceComparisons;

        IReadOnlyCollection<IFileComparison> IFile.TargetComparisons => TargetComparisons;

        ISubDirectory IFile.Parent => Parent;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        IUpstreamContentInfo IUpstreamFile.HashInfo => HashInfo;

        IUpstreamRedundancy IUpstreamFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        #endregion
    }
}
