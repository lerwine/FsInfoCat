using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VideoPropertiesListItem : VideoPropertiesRow, ILocalVideoPropertiesListItem, IEquatable<VideoPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vVideoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VideoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(Director)).HasConversion(MultiStringValue.Converter);
        }

        public bool Equals(VideoPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IVideoPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is VideoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalVideoPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IVideoPropertiesRow other)
        {
            if (other is null) return false;
            if (other is VideoPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalVideoPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IVideoProperties other)
        {
            if (other is null) return false;
            if (other is VideoPropertiesListItem listItem) return Equals(listItem);
            if (other is IVideoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalVideoPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is VideoPropertiesListItem listItem) return Equals(listItem);
            if (obj is IVideoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalVideoPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IVideoProperties properties && ArePropertiesEqual(properties);
        }
    }
}
