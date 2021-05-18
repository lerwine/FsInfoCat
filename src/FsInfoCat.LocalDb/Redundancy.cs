using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class Redundancy : ILocalRedundancy
    {
        public Redundancy()
        {
            Files = new HashSet<FsFile>();
        }

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(Id));
            //builder.ToTable($"{nameof(Redundancy)}{nameof(FsFile)}").OwnsMany(p => p.Files).HasForeignKey(k => k.Id)
            //    .OwnsMany(d => d.Redundancies).HasForeignKey(d => d.Id);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required()]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        [Required()]
        public DateTime ModifiedOn { get; set; }

        public Guid? UpstreamId { get; set; }

        public DateTime? LastSynchronized { get; set; }

        #endregion

        #region Navigation Properties

        public virtual HashSet<FsFile> Files { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IFile> IRedundancy.Files => Files;

        IReadOnlyCollection<ILocalFile> ILocalRedundancy.Files => Files;

        #endregion
    }
}
