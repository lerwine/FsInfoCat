using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
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

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public virtual HashSet<FsFile> Files { get; set; }

        IReadOnlyCollection<ILocalFile> ILocalRedundancy.Files => Files;

        IReadOnlyCollection<IFile> IRedundancy.Files => Files;

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(Id));
            builder.OwnsMany(p => p.Files).OwnsMany(f => f.Redundancies);
            //builder.ToTable($"{nameof(Redundancy)}{nameof(FsFile)}").OwnsMany(p => p.Files).HasForeignKey(k => k.Id)
            //    .OwnsMany(d => d.Redundancies).HasForeignKey(d => d.Id);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
