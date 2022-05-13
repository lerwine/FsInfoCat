using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SharedFileTag : ItemTag, ILocalSharedFileTag, ISharedFileTag, IEquatable<SharedFileTag>
    {
        #region Fields

        private readonly ForeignKeyReference<DbFile> _taggedNav;
        private readonly ForeignKeyReference<SharedTagDefinition> _definitionNav;

        #endregion

        #region Properties

        public override Guid TaggedId
        {
            get => _taggedNav.Id;
            set => _taggedNav.SetId(value);
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DbFile Tagged
        {
            get => _taggedNav.Entity;
            set => _taggedNav.Entity = value;
        }

        public override Guid DefinitionId
        {
            get => _definitionNav.Id;
            set => _definitionNav.SetId(value);
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public SharedTagDefinition Definition
        {
            get => _definitionNav.Entity;
            set => _definitionNav.Entity = value;
        }

        #endregion

        #region Explicit Members

        ILocalSharedTagDefinition ILocalSharedTag.Definition => Definition;

        ISharedTagDefinition ISharedTag.Definition => Definition;

        ILocalFile ILocalFileTag.Tagged => Tagged;

        IFile IFileTag.Tagged => Tagged;

        #endregion

        public SharedFileTag()
        {
            _taggedNav = new(null, SyncRoot);
            _definitionNav = new(null, SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<SharedFileTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.FileTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.SharedTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalSharedFileTag other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISharedFileTag other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SharedFileTag other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            DbFile b1 = Tagged;
            DbFile b2 = other.Tagged;
            if (b1 is null)
            {
                if (b2 is null)
                {
                    if (other.TaggedId.Equals(Guid.Empty))
                        return TaggedId.Equals(Guid.Empty) && ArePropertiesEqual(other);
                    return TaggedId.Equals(other.TaggedId);
                }
                return !TaggedId.Equals(Guid.Empty) && TaggedId.Equals(b2.Id);
            }
            if (b2 is null)
                return !other.TaggedId.Equals(Guid.Empty) && other.TaggedId.Equals(b1.Id);
            if (!b1.Equals(b2))
                return false;
            SharedTagDefinition d1 = Definition;
            SharedTagDefinition d2 = other.Definition;
            if (d1 is null)
            {
                if (d2 is null)
                {
                    if (other.DefinitionId.Equals(Guid.Empty))
                        return DefinitionId.Equals(Guid.Empty) && ArePropertiesEqual(other);
                    return DefinitionId.Equals(other.DefinitionId);
                }
                return !DefinitionId.Equals(Guid.Empty) && DefinitionId.Equals(d2.Id);
            }
            if (d2 is null)
                return !other.DefinitionId.Equals(Guid.Empty) && other.DefinitionId.Equals(d1.Id);
            return d1.Equals(d2);
        }

        public bool Equals(ISharedFileTag other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool TryGetDefinitionId(out Guid definitionId) => _definitionNav.TryGetId(out definitionId);

        public override bool TryGetTaggedId(out Guid taggedId) => _taggedNav.TryGetId(out taggedId);
    }
}
