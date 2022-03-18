using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SharedTagDefinitionListItem : SharedTagDefinitionRow, ILocalTagDefinitionListItem, IEquatable<SharedTagDefinitionListItem>
    {
        public const string VIEW_NAME = "vSharedTagDefinitionListing";

        public long FileTagCount { get; set; }

        public long SubdirectoryTagCount { get; set; }

        public long VolumeTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<SharedTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalTagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ITagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SharedTagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ITagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = hash * 19 + Name.GetHashCode();
                hash = hash * 19 + Description.GetHashCode();
                hash = EntityExtensions.HashNullable(UpstreamId, hash, 19);
                hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 19);
                hash = hash * 19 + CreatedOn.GetHashCode();
                hash = hash * 19 + ModifiedOn.GetHashCode();
                return hash;
            }
        }
    }
}
