using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class SymbolicName : ILocalSymbolicName
    {
        private string _name = "";
        private string _notes = "";

        public SymbolicName()
        {
            FileSystemDefaults = new HashSet<FileSystem>();
        }

        internal static void BuildEntity(EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_SimpleName).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(p => p.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            Validator.TryValidateProperty(FileSystem, new ValidationContext(this, null, null) { MemberName = nameof(FileSystem) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        public Guid FileSystemId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_FileSystem), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileSystemRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FileSystem FileSystem { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_FileSystemDefaults), ResourceType = typeof(ModelResources))]
        public virtual HashSet<FileSystem> FileSystemDefaults { get; set; }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem => FileSystem;

        IReadOnlyCollection<ILocalFileSystem> ILocalSymbolicName.FileSystemDefaults => FileSystemDefaults;

        IFileSystem IFsSymbolicName.FileSystem => FileSystem;

        IReadOnlyCollection<IFileSystem> IFsSymbolicName.FileSystemDefaults => FileSystemDefaults;

        #endregion
    }
}
