using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    // TODO: Document PersonalVolumeTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PersonalVolumeTag : ItemTag, IHasMembershipKeyReference<Volume, PersonalTagDefinition>, ILocalPersonalVolumeTag, IPersonalVolumeTag, IEquatable<PersonalVolumeTag>
    {
        private readonly VolumeReference _tagged;
        private readonly PersonalTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Volume), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Volume Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public PersonalTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalVolume ILocalVolumeTag.Tagged => Tagged;

        IVolume IVolumeTag.Tagged => Tagged;

        IForeignKeyReference<Volume> IHasMembershipKeyReference<Volume, PersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<PersonalTagDefinition> IHasMembershipKeyReference<Volume, PersonalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<IVolume> IHasMembershipKeyReference<IVolume, IPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<IPersonalTagDefinition> IHasMembershipKeyReference<IVolume, IPersonalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<IVolume> IHasMembershipKeyReference<IVolume, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<IVolume, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalVolume> IHasMembershipKeyReference<ILocalVolume, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalVolume, ILocalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalVolume> IHasMembershipKeyReference<ILocalVolume, ILocalPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalPersonalTagDefinition> IHasMembershipKeyReference<ILocalVolume, ILocalPersonalTagDefinition>.Ref2 => _definition;

        public PersonalVolumeTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalVolumeTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.VolumeTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalPersonalVolumeTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalVolumeTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalPersonalVolumeTag)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IPersonalVolumeTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IPersonalVolumeTag other)
        {
            // TODO: Implement ArePropertiesEqual(IPersonalVolumeTag)
            throw new NotImplementedException();
        }

        public bool Equals(PersonalVolumeTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

        public bool Equals(ILocalPersonalVolumeTag other) => other is not null && ((other is PersonalVolumeTag tag) ? Equals(tag) : ArePropertiesEqual(other));

        public bool Equals(ILocalVolumeTag other)
        {
            if (other is null) return false;
            if (other is PersonalVolumeTag tag) return Equals(tag);
            return other is ILocalPersonalVolumeTag local && ArePropertiesEqual(local);
        }

        public bool Equals(IPersonalVolumeTag other)
        {
            if (other is null) return false;
            if (other is PersonalVolumeTag tag) return Equals(tag);
            return (other is ILocalPersonalVolumeTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PersonalVolumeTag tag) return Equals(tag);
            return obj is IPersonalVolumeTag other && ((other is ILocalPersonalVolumeTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
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
