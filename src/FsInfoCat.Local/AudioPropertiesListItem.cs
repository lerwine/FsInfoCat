using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertiesListItem other) => ArePropertiesEqual((IAudioPropertiesListItem)other) &&
                   EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
                   LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IAudioPropertiesListItem other) => ArePropertiesEqual((IAudioProperties)other) && CreatedOn == other.CreatedOn &&
                   ModifiedOn == other.ModifiedOn &&
                   ExistingFileCount == other.ExistingFileCount &&
                   TotalFileCount == other.TotalFileCount;

        public bool Equals(AudioPropertiesListItem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (TryGetId(out Guid id))
                return other.TryGetId(out Guid g) && id.Equals(g);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(IAudioPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem audioPropertiesListItem) return Equals(audioPropertiesListItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (other is ILocalAudioPropertiesListItem localAudioPropertiesListItem)
                return ArePropertiesEqual(localAudioPropertiesListItem);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IAudioPropertiesRow other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem audioPropertiesListItem) return Equals(audioPropertiesListItem);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalAudioPropertiesListItem localListItem)
                return ArePropertiesEqual(localListItem);
            if (other is IAudioPropertiesListItem listItem)
                return ArePropertiesEqual(listItem);
            if (other is ILocalAudioPropertiesRow localRow)
                return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IAudioProperties other)
        {
            if (other is null) return false;
            if (other is AudioPropertiesListItem audioPropertiesListItem) return Equals(audioPropertiesListItem);
            if (other is IAudioPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalAudioPropertiesListItem localListItem)
                    return ArePropertiesEqual(localListItem);
                if (row is IAudioPropertiesListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is ILocalAudioPropertiesRow localRow)
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is AudioPropertiesListItem audioPropertiesListItem) return Equals(audioPropertiesListItem);
            if (obj is IAudioPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalAudioPropertiesListItem localListItem)
                    return ArePropertiesEqual(localListItem);
                if (row is IAudioPropertiesListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is ILocalAudioPropertiesRow localRow)
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IAudioProperties other && ArePropertiesEqual(other);
        }
    }
}
