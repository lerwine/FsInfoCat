using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class DirectoryRelocateTask : IDirectoryRelocateTask
    {
        private string _shortDescription = "";
        private string _notes = "";

        public DirectoryRelocateTask()
        {
            SourceDirectories = new HashSet<FsDirectory>();
        }

        internal static void BuildEntity(EntityTypeBuilder<DirectoryRelocateTask> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(ShortDescription)).HasMaxLength(DbConstants.DbColMaxLen_ShortDescription).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(p => p.TargetDirectory).WithMany(d => d.TargetDirectoryRelocationTasks).IsRequired();
            builder.HasOne(t => t.AssignmentGroup).WithMany(u => u.DirectoryRelocationTasks);
            builder.HasOne(t => t.AssignedTo).WithMany(u => u.DirectoryRelocationTasks);
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedDirectoryRelocateTasks).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedDirectoryRelocateTasks).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(ShortDescription, new ValidationContext(this, null, null) { MemberName = nameof(ShortDescription) }, results);
            Validator.TryValidateProperty(TargetDirectory, new ValidationContext(this, null, null) { MemberName = nameof(TargetDirectory) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required]
        public AppTaskStatus Status { get; set; }

        [Required]
        public PriorityLevel Priority { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ShortDescription), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ShortDescriptionRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_ShortDescription, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string ShortDescription { get => _shortDescription; set => _shortDescription = value ?? ""; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        public bool IsInactive { get; set; }

        public Guid TargetDirectoryId { get; set; }

        public Guid? AssignmentGroupId { get; set; }

        public Guid? AssignedToId { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_TargetDirectory), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_TargetDirectory), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FsDirectory TargetDirectory { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_AssignmentGroup), ResourceType = typeof(ModelResources))]
        public UserGroup AssignmentGroup { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_AssignedTo), ResourceType = typeof(ModelResources))]
        public UserProfile AssignedTo { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_SourceDirectories), ResourceType = typeof(ModelResources))]
        public virtual HashSet<FsDirectory> SourceDirectories { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IUpstreamSubDirectory> IDirectoryRelocateTask.SourceDirectories => SourceDirectories;

        IUpstreamSubDirectory IDirectoryRelocateTask.TargetDirectory => TargetDirectory;

        IUserGroup IDirectoryRelocateTask.AssignmentGroup => AssignmentGroup;

        IUserProfile IDirectoryRelocateTask.AssignedTo => AssignedTo;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
