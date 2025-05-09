using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity for a personal tag that can be associated with <see cref="DbFile"/>, <see cref="Subdirectory"/> or <see cref="Volume"/> entities.
    /// </summary>
    /// <seealso cref="PersonalTagDefinition" />
    /// <seealso cref="SharedTagDefinitionListItem" />
    /// <seealso cref="LocalDbContext.PersonalTagDefinitionListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class PersonalTagDefinitionListItem : PersonalTagDefinitionRow, ILocalTagDefinitionListItem, IEquatable<PersonalTagDefinitionListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vPersonalTagDefinitionListing";

        public long FileTagCount { get; set; }

        public long SubdirectoryTagCount { get; set; }

        public long VolumeTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(PersonalTagDefinitionListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ITagDefinitionListItem other)
        {
            if (other is null) return false;
            if (other is PersonalTagDefinitionListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalTagDefinitionListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    FileTagCount={FileTagCount}, SubdirectoryTagCount={SubdirectoryTagCount}, VolumeTagCount={VolumeTagCount}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
