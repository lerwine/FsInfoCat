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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
