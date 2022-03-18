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

        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IAudioPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(AudioPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IAudioPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IAudioProperties other)
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
