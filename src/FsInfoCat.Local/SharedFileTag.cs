using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public class SharedFileTag : ItemTag, IHasMembershipKeyReference<DbFile, SharedTagDefinition>, ILocalSharedFileTag, ISharedFileTag, IEquatable<SharedFileTag>
    {
        private readonly FileReference _tagged;
        private readonly SharedTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_tagged))]
        public DbFile Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_definition))]
        public SharedTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        ILocalSharedTagDefinition ILocalSharedTag.Definition => Definition;

        ISharedTagDefinition ISharedTag.Definition => Definition;

        ILocalFile ILocalFileTag.Tagged => Tagged;

        IFile IFileTag.Tagged => Tagged;

        IForeignKeyReference<DbFile> IHasMembershipKeyReference<DbFile, SharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<SharedTagDefinition> IHasMembershipKeyReference<DbFile, SharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, ISharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ISharedTagDefinition> IHasMembershipKeyReference<IFile, ISharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<IFile, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<ILocalFile, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalSharedTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        public SharedFileTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
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
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                // TODO: Implement Equals(object)
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }

        public override bool TryGetDefinitionId(out Guid definitionId) => _definition.TryGetId(out definitionId);

        public override bool TryGetTaggedId(out Guid taggedId) => _tagged.TryGetId(out taggedId);
    }
}
