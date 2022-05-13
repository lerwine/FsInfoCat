using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class MusicPropertiesListItem : MusicPropertiesRow, ILocalMusicPropertiesListItem, IEquatable<MusicPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vMusicPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<MusicPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
        }

        public bool Equals(MusicPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IMusicPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is MusicPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalMusicPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IMusicPropertiesRow other)
        {
            if (other is null) return false;
            if (other is MusicPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalMusicPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IMusicProperties other)
        {
            if (other is null) return false;
            if (other is MusicPropertiesListItem listItem) return Equals(listItem);
            if (other is IMusicPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalMusicPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is MusicPropertiesListItem listItem) return Equals(listItem);
            if (obj is IMusicPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalMusicPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is IMusicProperties properties && ArePropertiesEqual(properties);
        }
    }
}
