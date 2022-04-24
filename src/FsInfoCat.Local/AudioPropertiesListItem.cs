using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class AudioPropertiesListItem : AudioPropertiesRow, ILocalAudioPropertiesListItem, IEquatable<AudioPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vAudioPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<AudioPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(AudioPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IAudioPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalAudioPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IAudioPropertiesRow other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalAudioPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IAudioProperties other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem listItem) return Equals(listItem);
            if (other is IAudioPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalAudioPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is AudioPropertiesListItem listItem) return Equals(listItem);
            if (obj is IAudioPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalAudioPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IAudioProperties properties && ArePropertiesEqual(properties);
        }
    }
}
