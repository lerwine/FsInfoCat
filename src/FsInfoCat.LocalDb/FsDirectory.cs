using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
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
            builder.Property(nameof(Name)).HasMaxLength(DBSettings.Default.DbColMaxLen_FileSystemName).IsRequired();
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

        // TODO: [Id] uniqueidentifier  NOT NULL,
        public Guid Id { get; set; }

        // [Name] nvarchar(128)  NOT NULL,
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_FileSystemName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        // TODO: [CrawlFlags] tinyint  NOT NULL,
        public DirectoryCrawlFlags CrawlFlags { get; set; }

        // [ParentId] uniqueidentifier  NULL,
        public Guid? ParentId { get; set; }

        public Guid? UpstreamId { get; set; }

        public DateTime? LastSynchronized { get; set; }

        // TODO: [CreatedOn] datetime  NOT NULL,
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        // TODO: [ModifiedOn] datetime  NOT NULL
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Volume Volume { get; set; }

        public virtual HashSet<FsDirectory> SubDirectories { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
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
