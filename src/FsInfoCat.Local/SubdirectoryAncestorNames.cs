using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SubdirectoryAncestorNames : RevertibleChangeTracking, ISubdirectoryAncestorName
    {
        public const string VIEW_NAME = "vSubdirectoryAncestorNames";

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<Guid?> _parentId;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<string> _ancestorNames;

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        public virtual Guid? ParentId { get => _parentId.GetValue(); set => _parentId.SetValue(value); }

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryAncestorNames> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public SubdirectoryAncestorNames()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _parentId = AddChangeTracker<Guid?>(nameof(ParentId), null);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
        }
    }
}
