using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// DB entity for an <see cref="SharedTagDefinition"/> that is associated with an <see cref="Subdirectory"/>.
    /// </summary>
    /// <seealso cref="SharedSubdirectoryTagListItem" />
    /// <seealso cref="LocalDbContext.SharedSubdirectoryTags" />
    public class SharedSubdirectoryTag : ItemTag, IHasMembershipKeyReference<Subdirectory, SharedTagDefinition>, ILocalSharedSubdirectoryTag, IEquatable<SharedSubdirectoryTag>
    {
        private readonly SubdirectoryReference _tagged;
        private readonly SharedTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_SubdirectoryRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.TaggedSubdirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Subdirectory Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public SharedTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        ILocalSharedTagDefinition ILocalSharedTag.Definition => Definition;

        ISharedTagDefinition ISharedTag.Definition => Definition;

        ILocalSubdirectory ILocalSubdirectoryTag.Tagged => Tagged;

        ISubdirectory ISubdirectoryTag.Tagged => Tagged;

        IForeignKeyReference<Subdirectory> IHasMembershipKeyReference<Subdirectory, SharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<SharedTagDefinition> IHasMembershipKeyReference<Subdirectory, SharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<ISubdirectory> IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ISharedTagDefinition> IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ISubdirectory> IHasMembershipKeyReference<ISubdirectory, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<ISubdirectory, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalSubdirectory> IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalSubdirectory> IHasMembershipKeyReference<ILocalSubdirectory, ILocalSharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalSharedTagDefinition> IHasMembershipKeyReference<ILocalSubdirectory, ILocalSharedTagDefinition>.Ref2 => _definition;

        public SharedSubdirectoryTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.SubdirectoryTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.SharedTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalSharedSubdirectoryTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalSharedSubdirectoryTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalSharedSubdirectoryTag)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ISharedSubdirectoryTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ISharedSubdirectoryTag other)
        {
            // TODO: Implement ArePropertiesEqual(ISharedSubdirectoryTag)
            throw new NotImplementedException();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(SharedSubdirectoryTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

        public bool Equals(ILocalSharedSubdirectoryTag other) => other is not null && ((other is SharedSubdirectoryTag tag) ? Equals(tag) : ArePropertiesEqual(other));

        public bool Equals(ILocalSubdirectoryTag other)
        {
            if (other is null) return false;
            if (other is SharedSubdirectoryTag tag) return Equals(tag);
            return other is ILocalSharedSubdirectoryTag local && ArePropertiesEqual(local);
        }

        public bool Equals(ISharedSubdirectoryTag other)
        {
            if (other is null) return false;
            if (other is SharedSubdirectoryTag tag) return Equals(tag);
            return (other is ILocalSharedSubdirectoryTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SharedSubdirectoryTag tag) return Equals(tag);
            return obj is ISharedSubdirectoryTag other && ((other is ILocalSharedSubdirectoryTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
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

        public override string ToString() => $@"{{ TaggedId={_tagged.IdValue}, DefinitionId={_definition.IdValue},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        public override bool TryGetDefinitionId(out Guid definitionId) => _definition.TryGetId(out definitionId);

        public override bool TryGetTaggedId(out Guid taggedId) => _tagged.TryGetId(out taggedId);
    }
}
