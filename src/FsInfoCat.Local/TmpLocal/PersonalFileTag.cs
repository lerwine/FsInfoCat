using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    // TODO: Document PersonalFileTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PersonalFileTag : ItemTag, IHasMembershipKeyReference<DbFile, PersonalTagDefinition>, ILocalPersonalFileTag, IEquatable<PersonalFileTag>
    {
        private readonly FileReference _tagged;
        private readonly PersonalTagReference _definition;

        public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DbFile Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

        public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public PersonalTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

        IForeignKeyReference<DbFile> IHasMembershipKeyReference<DbFile, PersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<PersonalTagDefinition> IHasMembershipKeyReference<DbFile, PersonalTagDefinition>.Ref2 => _definition;

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        M.IPersonalTagDefinition M.IPersonalTag.Definition => Definition;

        ILocalFile ILocalFileTag.Tagged => Tagged;

        M.IFile M.IFileTag.Tagged => Tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<M.IFile> IHasMembershipKeyReference<M.IFile, M.IPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<M.IPersonalTagDefinition> IHasMembershipKeyReference<M.IFile, M.IPersonalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<M.IFile> IHasMembershipKeyReference<M.IFile, M.ITagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<M.ITagDefinition> IHasMembershipKeyReference<M.IFile, M.ITagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref2 => _definition;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>.Ref1 => _tagged;

        IForeignKeyReference<ILocalPersonalTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>.Ref2 => _definition;

        public PersonalFileTag()
        {
            _tagged = new(SyncRoot);
            _definition = new(SyncRoot);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.FileTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalPersonalFileTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalFileTag other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalPersonalFileTag)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="M.IPersonalFileTag" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] M.IPersonalFileTag other)
        {
            // TODO: Implement ArePropertiesEqual(M.IPersonalFileTag)
            throw new NotImplementedException();
        }

        public bool Equals(PersonalFileTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

        public bool Equals(ILocalPersonalFileTag other) => other is not null && ((other is PersonalFileTag tag) ? Equals(tag) : ArePropertiesEqual(other));

        public bool Equals(ILocalFileTag other)
        {
            if (other is null) return false;
            if (other is PersonalFileTag tag) return Equals(tag);
            return other is ILocalPersonalFileTag local && ArePropertiesEqual(local);
        }

        public bool Equals(M.IPersonalFileTag other)
        {
            if (other is null) return false;
            if (other is PersonalFileTag tag) return Equals(tag);
            return (other is ILocalPersonalFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PersonalFileTag tag) return Equals(tag);
            return obj is M.IPersonalFileTag other && ((other is ILocalPersonalFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
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
