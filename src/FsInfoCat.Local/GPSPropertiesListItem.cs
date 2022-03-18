using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class GPSPropertiesListItem : GPSPropertiesRow, ILocalGPSPropertiesListItem, IEquatable<GPSPropertiesListItem>
    {
        public const string VIEW_NAME = "vGPSPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<GPSPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(VersionID)).HasConversion(ByteValues.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalGPSPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IGPSPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(GPSPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IGPSPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IGPSProperties other)
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
                    int hash = 53;
                    hash = hash * 61 + AreaInformation.GetHashCode();
                    hash = EntityExtensions.HashNullable(LatitudeDegrees, hash, 61);
                    hash = EntityExtensions.HashNullable(LatitudeMinutes, hash, 61);
                    hash = EntityExtensions.HashNullable(LatitudeSeconds, hash, 61);
                    hash = hash * 61 + LatitudeRef.GetHashCode();
                    hash = EntityExtensions.HashNullable(LongitudeDegrees, hash, 61);
                    hash = EntityExtensions.HashNullable(LongitudeMinutes, hash, 61);
                    hash = EntityExtensions.HashNullable(LongitudeSeconds, hash, 61);
                    hash = hash * 61 + LongitudeRef.GetHashCode();
                    hash = hash * 61 + MeasureMode.GetHashCode();
                    hash = hash * 61 + ProcessingMethod.GetHashCode();
                    hash = EntityExtensions.HashObject(VersionID, hash, 61);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 61);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 61);
                    hash = hash * 61 + CreatedOn.GetHashCode();
                    hash = hash * 61 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
