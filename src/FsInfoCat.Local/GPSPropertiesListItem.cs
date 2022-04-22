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

        public override bool Equals(IGPSPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IGPSProperties other)
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
            if (TryGetId(out Guid id)) return id.GetHashCode();
            // TODO: Implement GetHashCode()
            throw new NotImplementedException();
        }
    }
}
