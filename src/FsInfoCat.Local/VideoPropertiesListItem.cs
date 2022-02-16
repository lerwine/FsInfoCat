using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class VideoPropertiesListItem : VideoPropertiesRow, ILocalVideoPropertiesListItem, IEquatable<VideoPropertiesListItem>
    {
        public const string VIEW_NAME = "vVideoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VideoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(Director)).HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(VideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVideoProperties other)
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
                    int hash = 43;
                    hash = hash * 53 + Compression.GetHashCode();
                    hash = (Director is null) ? hash * 53 : hash * 53 + (Director?.GetHashCode() ?? 0);
                    hash = EncodingBitrate.HasValue ? hash * 53 + (EncodingBitrate ?? default).GetHashCode() : hash * 53;
                    hash = FrameHeight.HasValue ? hash * 53 + (FrameHeight ?? default).GetHashCode() : hash * 53;
                    hash = FrameRate.HasValue ? hash * 53 + (FrameRate ?? default).GetHashCode() : hash * 53;
                    hash = FrameWidth.HasValue ? hash * 53 + (FrameWidth ?? default).GetHashCode() : hash * 53;
                    hash = HorizontalAspectRatio.HasValue ? hash * 53 + (HorizontalAspectRatio ?? default).GetHashCode() : hash * 53;
                    hash = hash * 53 + StreamName.GetHashCode();
                    hash = StreamNumber.HasValue ? hash * 53 + (StreamNumber ?? default).GetHashCode() : hash * 53;
                    hash = VerticalAspectRatio.HasValue ? hash * 53 + (VerticalAspectRatio ?? default).GetHashCode() : hash * 53;
                    hash = UpstreamId.HasValue ? hash * 53 + (UpstreamId ?? default).GetHashCode() : hash * 53;
                    hash = LastSynchronizedOn.HasValue ? hash * 53 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 53;
                    hash = hash * 53 + CreatedOn.GetHashCode();
                    hash = hash * 53 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
