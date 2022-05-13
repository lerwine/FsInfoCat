using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class PersonalVolumeTag : ItemTag, ILocalPersonalVolumeTag, IPersonalVolumeTag, IEquatable<PersonalVolumeTag>
    {
        #region Fields

        private readonly ForeignKeyReference<Volume> _taggedNav;
        private readonly ForeignKeyReference<PersonalTagDefinition> _definitionNav;

        #endregion

        #region Properties

        public override Guid TaggedId
        {
            get => _taggedNav.Id;
            set => _taggedNav.SetId(value);
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Volume), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Volume Tagged
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
        public PersonalTagDefinition Definition
        {
            get => _definitionNav.Entity;
            set => _definitionNav.Entity = value;
        }

        #endregion

        #region Explicit Members

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalVolume ILocalVolumeTag.Tagged => Tagged;

        IVolume IVolumeTag.Tagged => Tagged;

        #endregion

        public PersonalVolumeTag()
        {
            _taggedNav = new(null, SyncRoot);
            _definitionNav = new(null, SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalVolumeTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.VolumeTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalVolumeTag other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPersonalVolumeTag other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PersonalVolumeTag other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            Volume b1 = Tagged;
            Volume b2 = other.Tagged;
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
            PersonalTagDefinition d1 = Definition;
            PersonalTagDefinition d2 = other.Definition;
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

        public bool Equals(IPersonalVolumeTag other)
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
