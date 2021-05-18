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
    public class SymbolicName : ILocalSymbolicName
    {
        private string _name = "";
        private string _notes = "";

        public SymbolicName()
        {
            DefaultFileSystems = new HashSet<FileSystem>();
        }

        internal static void BuildEntity(EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DBSettings.Default.DbColMaxLen_SimpleName).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("");
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
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_SimpleName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        public Guid FileSystemId { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_FileSystem), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileSystemRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FileSystem FileSystem { get; set; }

        public virtual HashSet<FileSystem> DefaultFileSystems { get; set; }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem => FileSystem;

        IReadOnlyCollection<ILocalFileSystem> ILocalSymbolicName.DefaultFileSystems => DefaultFileSystems;

        IFileSystem IFsSymbolicName.FileSystem => FileSystem;

        IReadOnlyCollection<IFileSystem> IFsSymbolicName.DefaultFileSystems => DefaultFileSystems;

        #endregion
    }
}
