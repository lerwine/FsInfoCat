using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    // TODO: Document SharedFileTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SharedFileTag : ItemTag, IHasMembershipKeyReference<DbFile, SharedTagDefinition>, ILocalSharedFileTag, ISharedFileTag, IEquatable<SharedFileTag>
    {
        private readonly FileReference _tagged;
        private readonly SharedTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DbFile Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref2 => _definition;

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
        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalSharedFileTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>

        protected bool ArePropertiesEqual([DisallowNull] ILocalSharedFileTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalSharedFileTag)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ISharedFileTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ISharedFileTag other)
        {
            // TODO: Implement ArePropertiesEqual(ISharedFileTag)
            throw new NotImplementedException();
        }


        public bool Equals(SharedFileTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

        public bool Equals(ILocalSharedFileTag other) => other is not null && ((other is SharedFileTag tag) ? Equals(tag) : ArePropertiesEqual(other));

        public bool Equals(ILocalFileTag other)
        {
            if (other is null) return false;
            if (other is SharedFileTag tag) return Equals(tag);
            return other is ILocalSharedFileTag local && ArePropertiesEqual(local);
        }

        public bool Equals(ISharedFileTag other)
        {
            if (other is null) return false;
            if (other is SharedFileTag tag) return Equals(tag);
            return (other is ILocalSharedFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SharedFileTag tag) return Equals(tag);
            return obj is ISharedFileTag other && ((other is ILocalSharedFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
        }

        public override int GetHashCode()
        {
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                // TODO: Implement GetHashCode()
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }

        public override bool TryGetDefinitionId(out Guid definitionId) => _definition.TryGetId(out definitionId);

        public override bool TryGetTaggedId(out Guid taggedId) => _tagged.TryGetId(out taggedId);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
