using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public abstract class SymbolicNameRow : LocalDbEntity, ILocalSymbolicNameRow, ISimpleIdentityReference<SymbolicNameRow>
    {
        #region Fields

        public const string Fallback_Symbolic_Name = "NTFS";
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<int> _priority;
        private readonly IPropertyChangeTracker<bool> _isInactive;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id.GetValue();
            set
            {
                if (_id.IsSet)
                {
                    Guid id = _id.GetValue();
                    if (id.Equals(value))
                        return;
                    if (!id.Equals(Guid.Empty))
                        throw new InvalidOperationException();
                }
                _id.SetValue(value);
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Name), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Priority), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual int Priority { get => _priority.GetValue(); set => _priority.SetValue(value); }

        [Required]
        public virtual Guid FileSystemId
        {
            get => _fileSystemId.GetValue();
            set
            {
                if (_fileSystemId.SetValue(value))
                    OnFileSystemIdChanged(value);
            }
        }

        SymbolicNameRow IIdentityReference<SymbolicNameRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        protected virtual void OnFileSystemIdChanged(Guid value) { }

        #endregion

        protected SymbolicNameRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", TrimmedNonNullStringCoersion.Default);
            _priority = AddChangeTracker(nameof(Priority), 0);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
            _fileSystemId = AddChangeTracker(nameof(FileSystemId), Guid.Empty);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName) || validationContext.MemberName == nameof(Name))
            {
                string name = Name;
                LocalDbContext dbContext;
                using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
                if (string.IsNullOrEmpty(name) || (dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is null)
                    return;
                Guid id = Id;
                if (dbContext.SymbolicNames.Any(sn => id != sn.Id && sn.Name == name))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
            }
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
