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
    public class FsSymbolicName : ILocalSymbolicName, IValidatableObject
    {
        public FsSymbolicName()
        {
            DefaultFileSystems = new HashSet<FileSystem>();
        }

        public Guid Id { get; set; }

        private string _name = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public Guid FileSystemId { get; set; }

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_IS_INACTIVE)]
        public bool IsInactive { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = Constants.ERROR_MESSAGE_FILESYSTEM)]
        [DisplayName(Constants.DISPLAY_NAME_FILESYSTEM)]
        public virtual FileSystem FileSystem { get; set; }

        public virtual HashSet<FileSystem> DefaultFileSystems { get; set; }

        ILocalFileSystem ILocalSymbolicName.FileSystem => throw new NotImplementedException();

        IReadOnlyCollection<ILocalFileSystem> ILocalSymbolicName.DefaultFileSystems => throw new NotImplementedException();

        IFileSystem IFsSymbolicName.FileSystem => throw new NotImplementedException();

        IReadOnlyCollection<IFileSystem> IFsSymbolicName.DefaultFileSystems => throw new NotImplementedException();

        internal static void BuildEntity(EntityTypeBuilder<FsSymbolicName> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_NAME).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(p => p.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            Validator.TryValidateProperty(FileSystem, new ValidationContext(this, null, null) { MemberName = nameof(FileSystem) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
