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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
