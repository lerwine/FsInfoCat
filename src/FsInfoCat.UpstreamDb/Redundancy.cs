using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class Redundancy : IUpstreamRedundancy
    {
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(Id));
            builder.HasOne(d => d.TargetFile).WithOne(f => f.Redundancy).HasForeignKey(nameof(FileId)).IsRequired();
            builder.HasOne(d => d.RedundantSet).WithMany(r => r.Redundancies).HasForeignKey(nameof(RedundantSetId)).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedRedundancies).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedRedundancies).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        #region Column Properties

        public Guid Id { get; set; }

        public Guid RedundantSetId { get; set; }

        public Guid FileId { get; set; }

        [Required]
        public FileRedundancyStatus Status { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

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

        [Display(Name = nameof(ModelResources.DisplayName_TargetFile), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile TargetFile { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_RedundancySet), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_RedundancySetRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public RedundantSet RedundantSet => throw new NotImplementedException();

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        IUpstreamFile IUpstreamRedundancy.TargetFile => TargetFile;

        IFile IRedundancy.TargetFile => TargetFile;

        IUpstreamRedundantSet IUpstreamRedundancy.RedundantSet => RedundantSet;

        IRedundantSet IRedundancy.RedundantSet => RedundantSet;

        #endregion
    }
}
