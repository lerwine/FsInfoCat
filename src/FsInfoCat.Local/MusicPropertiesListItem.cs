using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class MusicPropertiesListItem : MusicPropertiesRow, ILocalMusicPropertiesListItem, IEquatable<MusicPropertiesListItem>
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalMusicPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IMusicPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(MusicPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IMusicPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IMusicProperties other)
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
                    int hash = 47;
                    hash = hash * 59 + AlbumArtist.GetHashCode();
                    hash = hash * 59 + AlbumTitle.GetHashCode();
                    hash = (Artist is null) ? hash * 59 : hash * 59 + (Artist?.GetHashCode() ?? 0);
                    hash = ChannelCount.HasValue ? hash * 59 + (ChannelCount ?? default).GetHashCode() : hash * 59;
                    hash = (Composer is null) ? hash * 59 : hash * 59 + (Composer?.GetHashCode() ?? 0);
                    hash = (Conductor is null) ? hash * 59 : hash * 59 + (Conductor?.GetHashCode() ?? 0);
                    hash = hash * 59 + DisplayArtist.GetHashCode();
                    hash = (Genre is null) ? hash * 59 : hash * 59 + (Genre?.GetHashCode() ?? 0);
                    hash = hash * 59 + PartOfSet.GetHashCode();
                    hash = hash * 59 + Period.GetHashCode();
                    hash = TrackNumber.HasValue ? hash * 59 + (TrackNumber ?? default).GetHashCode() : hash * 59;
                    hash = UpstreamId.HasValue ? hash * 59 + (UpstreamId ?? default).GetHashCode() : hash * 59;
                    hash = LastSynchronizedOn.HasValue ? hash * 59 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 59;
                    hash = hash * 59 + CreatedOn.GetHashCode();
                    hash = hash * 59 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
