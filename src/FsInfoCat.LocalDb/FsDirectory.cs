using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.LocalDb
{
    [Table(TABLE_NAME)]
    public class FsDirectory : ILocalSubDirectory, IValidatableObject
    {
        public const string TABLE_NAME = "Directories";

        private string _name = "";
        private string _notes = "";

        public FsDirectory()
        {
            SubDirectories = new HashSet<FsDirectory>();
            Files = new HashSet<FsFile>();
        }

        internal static void BuildEntity(EntityTypeBuilder<FsDirectory> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_FileSystemName).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            if (Volume is null && Parent is null)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_NoParentOrVolume, new string[] { nameof(ModifiedOn) }));
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
        public DirectoryCrawlFlags Options { get; set; }

        public Guid? ParentId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        public bool Deleted { get; set; }

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
        public virtual Volume Volume { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_SubDirectories), ResourceType = typeof(ModelResources))]
        public virtual HashSet<FsDirectory> SubDirectories { get; set; }

        public virtual FsDirectory Parent { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
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
