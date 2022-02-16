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

        public bool Equals(AudioPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 37;
                    hash = hash * 43 + Compression.GetHashCode();
                    hash = EncodingBitrate.HasValue ? hash * 43 + EncodingBitrate.Value.GetHashCode() : hash * 43;
                    hash = hash * 43 + Format.GetHashCode();
                    hash = IsVariableBitrate.HasValue ? hash * 43 + IsVariableBitrate.Value.GetHashCode() : hash * 43;
                    hash = SampleRate.HasValue ? hash * 43 + SampleRate.Value.GetHashCode() : hash * 43;
                    hash = SampleSize.HasValue ? hash * 43 + SampleSize.Value.GetHashCode() : hash * 43;
                    hash = hash * 43 + StreamName.GetHashCode();
                    hash = StreamNumber.HasValue ? hash * 43 + StreamNumber.Value.GetHashCode() : hash * 43;
                    hash = UpstreamId.HasValue ? hash * 43 + UpstreamId.Value.GetHashCode() : hash * 43;
                    hash = LastSynchronizedOn.HasValue ? hash * 43 + LastSynchronizedOn.Value.GetHashCode() : hash * 43;
                    hash = hash * 43 + CreatedOn.GetHashCode();
                    hash = hash * 43 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
