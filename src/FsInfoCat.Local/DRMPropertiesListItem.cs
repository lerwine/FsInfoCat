using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class DRMPropertiesListItem : DRMPropertiesRow, ILocalDRMPropertiesListItem, IEquatable<DRMPropertiesListItem>
    {
        public const string VIEW_NAME = "vDRMPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DRMPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DRMPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDRMProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 23;
                    hash = EntityExtensions.HashNullable(DatePlayExpires, hash, 31);
                    hash = EntityExtensions.HashNullable(DatePlayStarts, hash, 31);
                    hash = hash * 31 + Description.GetHashCode();
                    hash = hash * 31 + IsProtected.GetHashCode();
                    hash = EntityExtensions.HashNullable(PlayCount, hash, 31);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 31);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 31);
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
