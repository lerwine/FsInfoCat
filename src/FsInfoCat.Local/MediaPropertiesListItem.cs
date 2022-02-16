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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 61;
                    hash = hash * 71 + ContentDistributor.GetHashCode();
                    hash = hash * 71 + CreatorApplication.GetHashCode();
                    hash = hash * 71 + CreatorApplicationVersion.GetHashCode();
                    hash = hash * 71 + DateReleased.GetHashCode();
                    hash = Duration.HasValue ? hash * 71 + (Duration ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + DVDID.GetHashCode();
                    hash = FrameCount.HasValue ? hash * 71 + (FrameCount ?? default).GetHashCode() : hash * 71;
                    hash = (Producer is null) ? hash * 71 : hash * 71 + (Producer?.GetHashCode() ?? 0);
                    hash = hash * 71 + ProtectionType.GetHashCode();
                    hash = hash * 71 + ProviderRating.GetHashCode();
                    hash = hash * 71 + ProviderStyle.GetHashCode();
                    hash = hash * 71 + Publisher.GetHashCode();
                    hash = hash * 71 + Subtitle.GetHashCode();
                    hash = (Writer is null) ? hash * 71 : hash * 71 + (Writer?.GetHashCode() ?? 0);
                    hash = UpstreamId.HasValue ? hash * 71 + (UpstreamId ?? default).GetHashCode() : hash * 71;
                    hash = LastSynchronizedOn.HasValue ? hash * 71 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
