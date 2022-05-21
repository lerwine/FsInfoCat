using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    // TODO: Document SharedVolumeTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SharedVolumeTag : ItemTag, IHasMembershipKeyReference<Volume, SharedTagDefinition>, ILocalSharedVolumeTag, IEquatable<SharedVolumeTag>
    {
        private readonly VolumeReference _tagged;
        private readonly SharedTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Volume), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Volume Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public SharedTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        ILocalSharedTagDefinition ILocalSharedTag.Definition => Definition;

        ISharedTagDefinition ISharedTag.Definition => Definition;

        ILocalVolume ILocalVolumeTag.Tagged => Tagged;

        IVolume IVolumeTag.Tagged => Tagged;

        IForeignKeyReference<Volume> IHasMembershipKeyReference<Volume, SharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<SharedTagDefinition> IHasMembershipKeyReference<Volume, SharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<IVolume> IHasMembershipKeyReference<IVolume, ISharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ISharedTagDefinition> IHasMembershipKeyReference<IVolume, ISharedTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<IVolume> IHasMembershipKeyReference<IVolume, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<IVolume, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalVolume> IHasMembershipKeyReference<ILocalVolume, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalVolume, ILocalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalVolume> IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalSharedTagDefinition> IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition>.Ref2 => _definition;

        public SharedVolumeTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<SharedVolumeTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.VolumeTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.SharedTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalSharedVolumeTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalSharedVolumeTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalSharedVolumeTag)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ISharedVolumeTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ISharedVolumeTag other)
        {
            // TODO: Implement ArePropertiesEqual(ISharedVolumeTag)
            throw new NotImplementedException();
        }

        public bool Equals(SharedVolumeTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

        public bool Equals(ILocalSharedVolumeTag other) => other is not null && ((other is SharedVolumeTag tag) ? Equals(tag) : ArePropertiesEqual(other));

        public bool Equals(ILocalVolumeTag other)
        {
            if (other is null) return false;
            if (other is SharedVolumeTag tag) return Equals(tag);
            return other is ILocalSharedVolumeTag local && ArePropertiesEqual(local);
        }

        public bool Equals(ISharedVolumeTag other)
        {
            if (other is null) return false;
            if (other is SharedVolumeTag tag) return Equals(tag);
            return (other is ILocalSharedVolumeTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SharedVolumeTag tag) return Equals(tag);
            return obj is ISharedVolumeTag other && ((other is ILocalSharedVolumeTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
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
