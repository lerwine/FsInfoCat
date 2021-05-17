using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FsDirectory : ILocalSubDirectory, IValidatableObject
    {
        private string _name = "";

        public FsDirectory()
        {
            SubDirectories = new HashSet<FsDirectory>();
            Files = new HashSet<FsFile>();
        }

        internal static void BuildEntity(EntityTypeBuilder<FsDirectory> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            if (Volume is null && Parent is null)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_VOLUME_PARENT_REQUIRED, new string[] { nameof(Volume), nameof(Parent) }));
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public DirectoryCrawlFlags CrawlFlags { get; set; }

        public Guid? ParentId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Volume Volume { get; set; }

        public virtual HashSet<FsDirectory> SubDirectories { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public virtual FsDirectory Parent { get; set; }

        public virtual HashSet<FsFile> Files { get; set; }

        #endregion

        #region Explicit Members

        IVolume ISubDirectory.Volume => Volume;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => SubDirectories;

        ISubDirectory ISubDirectory.Parent => Parent;

        IReadOnlyCollection<IFile> ISubDirectory.Files => Files;

        ILocalSubDirectory ILocalSubDirectory.Parent => Parent;

        ILocalVolume ILocalSubDirectory.Volume => Volume;

        IReadOnlyCollection<ILocalFile> ILocalSubDirectory.Files => Files;

        IReadOnlyCollection<ILocalSubDirectory> ILocalSubDirectory.SubDirectories => SubDirectories;

        #endregion
    }
}
