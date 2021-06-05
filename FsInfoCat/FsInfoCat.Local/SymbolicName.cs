using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SymbolicName : LocalDbEntity, ILocalSymbolicName
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<int> _priority;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        /// <remarks>TEXT NOT NULL DEFAULT ''</remarks>
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        [Required]
        public virtual int Priority { get => _priority.GetValue(); set => _priority.SetValue(value); }

        [Required]
        public virtual Guid FileSystemId
        {
            get => _fileSystemId.GetValue();
            set
            {
                if (_fileSystemId.SetValue(value))
                {
                    FileSystem nav = _fileSystem.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _fileSystem.SetValue(null);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem.GetValue();
            set
            {
                if (_fileSystem.SetValue(value))
                {
                    if (value is null)
                        _fileSystemId.SetValue(Guid.Empty);
                    else
                        _fileSystemId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        IFileSystem ISymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        #endregion

        public SymbolicName()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", TrimmedNonNullStringCoersion.Default);
            _priority = AddChangeTracker(nameof(Priority), 0);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
            _fileSystemId = AddChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = AddChangeTracker<FileSystem>(nameof(FileSystem), null);
        }

        internal static void BuildEntity([NotNull] EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            // TODO: Implement OnValidate(ValidationContext, List{ValidationResult})
            base.OnValidate(validationContext, results);
        }
        
        private void OnValidate([NotNull] EntityEntry<SymbolicName> entityEntry, [NotNull] LocalDbContext dbContext, [NotNull] List<ValidationResult> validationResults)
        {
            string name = Name;
            Guid id = Id;
            if (string.IsNullOrEmpty(name))
                return;
            var entities = from sn in dbContext.SymbolicNames where id != sn.Id && sn.Name == name select sn;
            if (entities.Any())
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
        }
    }
}
