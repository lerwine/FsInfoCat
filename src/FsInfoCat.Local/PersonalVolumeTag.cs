using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public class PersonalVolumeTag : ItemTag, ILocalPersonalVolumeTag, IPersonalVolumeTag, IEquatable<PersonalVolumeTag>
    {
        private Volume _tagged;
        private PersonalTagDefinition _definition;

        public override Guid TaggedId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _tagged?.Id;
                    if (id.HasValue && id.Value != base.TaggedId)
                    {
                        base.TaggedId = id.Value;
                        return id.Value;
                    }
                    return base.TaggedId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _tagged?.Id;
                    if (id.HasValue && id.Value != value)
                        _tagged = null;
                    base.TaggedId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Volume), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_tagged))]
        public Volume Tagged
        {
            get => _tagged;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_tagged is not null)
                            base.TaggedId = Guid.Empty;
                    }
                    else
                        base.TaggedId = value.Id;
                    _tagged = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid DefinitionId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _definition?.Id;
                    if (id.HasValue && id.Value != base.DefinitionId)
                    {
                        base.DefinitionId = id.Value;
                        return id.Value;
                    }
                    return base.DefinitionId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _definition?.Id;
                    if (id.HasValue && id.Value != value)
                        _definition = null;
                    base.DefinitionId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_definition))]
        public PersonalTagDefinition Definition
        {
            get => _definition;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_definition is not null)
                            base.DefinitionId = Guid.Empty;
                    }
                    else
                        base.DefinitionId = value.Id;
                    _definition = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalVolume ILocalVolumeTag.Tagged => Tagged;

        IVolume IVolumeTag.Tagged => Tagged;

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
            Guid taggedId = TaggedId;
            Guid definitionId = DefinitionId;
            if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
                // TODO: Implement Equals(object)
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }
    }
}
