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

        public bool Equals(GPSPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 53;
                    hash = hash * 61 + AreaInformation.GetHashCode();
                    hash = LatitudeDegrees.HasValue ? hash * 61 + (LatitudeDegrees ?? default).GetHashCode() : hash * 61;
                    hash = LatitudeMinutes.HasValue ? hash * 61 + (LatitudeMinutes ?? default).GetHashCode() : hash * 61;
                    hash = LatitudeSeconds.HasValue ? hash * 61 + (LatitudeSeconds ?? default).GetHashCode() : hash * 61;
                    hash = hash * 61 + LatitudeRef.GetHashCode();
                    hash = LongitudeDegrees.HasValue ? hash * 61 + (LongitudeDegrees ?? default).GetHashCode() : hash * 61;
                    hash = LongitudeMinutes.HasValue ? hash * 61 + (LongitudeMinutes ?? default).GetHashCode() : hash * 61;
                    hash = LongitudeSeconds.HasValue ? hash * 61 + (LongitudeSeconds ?? default).GetHashCode() : hash * 61;
                    hash = hash * 61 + LongitudeRef.GetHashCode();
                    hash = hash * 61 + MeasureMode.GetHashCode();
                    hash = hash * 61 + ProcessingMethod.GetHashCode();
                    hash = (VersionID is null) ? hash * 61 : hash * 61 + (VersionID?.GetHashCode() ?? 0);
                    hash = UpstreamId.HasValue ? hash * 61 + (UpstreamId ?? default).GetHashCode() : hash * 61;
                    hash = LastSynchronizedOn.HasValue ? hash * 61 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 61;
                    hash = hash * 61 + CreatedOn.GetHashCode();
                    hash = hash * 61 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
