using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class SharedTagDefinitionRow : LocalDbEntity, ILocalTagDefinitionRow
    {
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool> _isInactive;

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }

        [Required]
        public bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public SharedTagDefinitionRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _description = AddChangeTracker(nameof(Description), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }
    }
}
