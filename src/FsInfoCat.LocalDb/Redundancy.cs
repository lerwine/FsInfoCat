using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class Redundancy : ILocalRedundancy
    {
        private string _notes = "";

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(RedundantSetId), nameof(FileId));
            builder.HasOne(d => d.TargetFile).WithOne(f => f.Redundancy).HasForeignKey(nameof(FileId)).IsRequired();
            builder.HasOne(d => d.RedundantSet).WithMany(r => r.Redundancies).HasForeignKey(nameof(RedundantSetId)).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        //public Guid Id { get; set; }

        public Guid RedundantSetId { get; set; }

        public Guid FileId { get; set; }

        [Required]
        public FileRedundancyStatus Status { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

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

        [Display(Name = nameof(ModelResources.DisplayName_TargetFile), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public FsFile TargetFile { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_RedundancySet), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_RedundancySetRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public RedundantSet RedundantSet { get; set; }

        #endregion

        #region Explicit Members

        ILocalFile ILocalRedundancy.TargetFile => TargetFile;

        IFile IRedundancy.TargetFile => TargetFile;

        ILocalRedundantSet ILocalRedundancy.RedundantSet => RedundantSet;

        IRedundantSet IRedundancy.RedundantSet => RedundantSet;

        #endregion
    }
}
