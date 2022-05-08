using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public class PersonalSubdirectoryTag : ItemTag, IHasMembershipKeyReference<Subdirectory, PersonalTagDefinition>, ILocalPersonalSubdirectoryTag, IPersonalSubdirectoryTag, IEquatable<PersonalSubdirectoryTag>
    {
        private readonly SubdirectoryReference _tagged;
        private readonly PersonalTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_SubdirectoryRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Subdirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Subdirectory Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public PersonalTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalSubdirectory ILocalSubdirectoryTag.Tagged => Tagged;

        ISubdirectory ISubdirectoryTag.Tagged => Tagged;

        IForeignKeyReference<Subdirectory> IHasMembershipKeyReference<Subdirectory, PersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<PersonalTagDefinition> IHasMembershipKeyReference<Subdirectory, PersonalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<ISubdirectory> IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<IPersonalTagDefinition> IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ISubdirectory> IHasMembershipKeyReference<ISubdirectory, ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<ISubdirectory, ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalSubdirectory> IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalSubdirectory> IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalPersonalTagDefinition> IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition>.Ref2 => _definition;

        public PersonalSubdirectoryTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.SubdirectoryTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalSubdirectoryTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalPersonalSubdirectoryTag)
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPersonalSubdirectoryTag other)
        {
            // TODO: Implement ArePropertiesEqual(IPersonalSubdirectoryTag)
            throw new NotImplementedException();
        }

        public bool Equals(PersonalSubdirectoryTag other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            Subdirectory b1 = Tagged;
            Subdirectory b2 = other.Tagged;
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

        public bool Equals(IPersonalSubdirectoryTag other)
        {
            // TODO: Implement Equals(IPersonalSubdirectoryTag)
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
                // TODO: Implement GetHashCode()
                throw new NotImplementedException();
            return HashCode.Combine(taggedId, definitionId);
        }

        public override bool TryGetDefinitionId(out Guid definitionId) => _definition.TryGetId(out definitionId);

        public override bool TryGetTaggedId(out Guid taggedId) => _tagged.TryGetId(out taggedId);
    }
}
