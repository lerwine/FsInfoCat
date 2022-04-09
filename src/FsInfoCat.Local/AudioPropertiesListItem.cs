using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class AudioPropertiesListItem : AudioPropertiesRow, ILocalAudioPropertiesListItem, IEquatable<AudioPropertiesListItem>
    {
        public const string VIEW_NAME = "vAudioPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<AudioPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertiesListItem other) =>
            EntityExtensions.NullablesEqual(UpstreamId, other.UpstreamId) && EntityExtensions.NullablesEqual(LastSynchronizedOn, other.LastSynchronizedOn) && ArePropertiesEqual((IAudioPropertiesListItem)other);

        protected bool ArePropertiesEqual([DisallowNull] IAudioPropertiesListItem other) => ExistingFileCount == other.ExistingFileCount && TotalFileCount == other.TotalFileCount && base.ArePropertiesEqual(other);

        public bool Equals(AudioPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? (other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other)) : Id.Equals(other.Id);

        public bool Equals(IAudioPropertiesListItem other) => other is not null && (other is AudioPropertiesListItem audioPropertiesListItem) ? Equals(audioPropertiesListItem) :
            Id.Equals(Guid.Empty) ? (other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other)) : Id.Equals(other.Id);

        public override bool Equals(IAudioPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IAudioProperties other)
        {
            if (other is null)
                return false;
            if (other is AudioPropertiesListItem audioPropertiesListItem)
                return Equals(audioPropertiesListItem);
            if (other is ILocalAudioPropertiesListItem localAudioPropertiesListItem)
                return Equals(localAudioPropertiesListItem);
            if (other is IAudioPropertiesListItem propertiesListItem)
                return Id.Equals(Guid.Empty) ? (propertiesListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(propertiesListItem)) : Id.Equals(propertiesListItem.Id);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is AudioPropertiesListItem audioPropertiesListItem)
                return Equals(audioPropertiesListItem);
            if (obj is ILocalAudioPropertiesListItem localAudioPropertiesListItem)
                return Equals(localAudioPropertiesListItem);
            if (obj is IAudioPropertiesListItem propertiesListItem)
                return Id.Equals(Guid.Empty) ? (propertiesListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(propertiesListItem)) : Id.Equals(propertiesListItem.Id);
            return obj is IAudioProperties other && ArePropertiesEqual(other);
        }

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 37;
                    hash = hash * 43 + Compression.GetHashCode();
                    hash = EntityExtensions.HashNullable(EncodingBitrate, hash, 43);
                    hash = hash * 43 + Format.GetHashCode();
                    hash = EntityExtensions.HashNullable(IsVariableBitrate, hash, 43);
                    hash = EntityExtensions.HashNullable(SampleRate, hash, 43);
                    hash = EntityExtensions.HashNullable(SampleSize, hash, 43);
                    hash = hash * 43 + StreamName.GetHashCode();
                    hash = EntityExtensions.HashNullable(StreamNumber, hash, 43);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 43);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 43);
                    hash = hash * 43 + CreatedOn.GetHashCode();
                    hash = hash * 43 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
