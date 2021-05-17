using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class Redundancy : IRemoteRedundancy
    {
        public Redundancy()
        {
            Files = new HashSet<FsFile>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(Id));
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedRedundancies).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedRedundancies).IsRequired();
            //builder.ToTable($"{nameof(Redundancy)}{nameof(FsFile)}").OwnsMany(p => p.Files).HasForeignKey(k => k.Id)
            //    .OwnsMany(d => d.Redundancies).HasForeignKey(d => d.Id);
        }

        #region Column Properties

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        [Required]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        [Required]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<FsFile> Files { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        IReadOnlyCollection<IRemoteFile> IRemoteRedundancy.Files => Files;

        IReadOnlyCollection<IFile> IRedundancy.Files => Files;

        #endregion
    }
}
