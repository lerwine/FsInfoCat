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

        public override bool Equals(IMusicPropertiesRow other)
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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 47;
                    hash = hash * 59 + AlbumArtist.GetHashCode();
                    hash = hash * 59 + AlbumTitle.GetHashCode();
                    hash = EntityExtensions.HashObject(Artist, hash, 59);
                    hash = EntityExtensions.HashNullable(ChannelCount, hash, 59);
                    hash = EntityExtensions.HashObject(Composer, hash, 59);
                    hash = EntityExtensions.HashObject(Conductor, hash, 59);
                    hash = hash * 59 + DisplayArtist.GetHashCode();
                    hash = EntityExtensions.HashObject(Genre, hash, 59);
                    hash = hash * 59 + PartOfSet.GetHashCode();
                    hash = hash * 59 + Period.GetHashCode();
                    hash = EntityExtensions.HashNullable(TrackNumber, hash, 59);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 59);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 59);
                    hash = hash * 59 + CreatedOn.GetHashCode();
                    hash = hash * 59 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
