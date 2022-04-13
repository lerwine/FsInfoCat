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

        public bool Equals(AudioPropertiesListItem other) => (other is not null && ReferenceEquals(this, other)) || Id.Equals(Guid.Empty) ? (other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other)) :
            Id.Equals(other.Id);

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
    }
}
