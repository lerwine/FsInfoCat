using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class MediaPropertiesListItem : MediaPropertiesRow, ILocalMediaPropertiesListItem, IEquatable<MediaPropertiesListItem>
    {
        public const string VIEW_NAME = "vMediaPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<MediaPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalMediaPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IMediaPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(MediaPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IMediaPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IMediaPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IMediaProperties other)
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
                    int hash = 61;
                    hash = hash * 71 + ContentDistributor.GetHashCode();
                    hash = hash * 71 + CreatorApplication.GetHashCode();
                    hash = hash * 71 + CreatorApplicationVersion.GetHashCode();
                    hash = hash * 71 + DateReleased.GetHashCode();
                    hash = EntityExtensions.HashNullable(Duration, hash, 71);
                    hash = hash * 71 + DVDID.GetHashCode();
                    hash = EntityExtensions.HashNullable(FrameCount, hash, 71);
                    hash = EntityExtensions.HashObject(Producer, hash, 71);
                    hash = hash * 71 + ProtectionType.GetHashCode();
                    hash = hash * 71 + ProviderRating.GetHashCode();
                    hash = hash * 71 + ProviderStyle.GetHashCode();
                    hash = hash * 71 + Publisher.GetHashCode();
                    hash = hash * 71 + Subtitle.GetHashCode();
                    hash = EntityExtensions.HashObject(Writer, hash, 71);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 71);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 71);
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
