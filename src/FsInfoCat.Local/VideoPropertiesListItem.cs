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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 43;
                    hash = hash * 53 + Compression.GetHashCode();
                    hash = EntityExtensions.HashObject(Director, hash, 53);
                    hash = EntityExtensions.HashNullable(EncodingBitrate, hash, 53);
                    hash = EntityExtensions.HashNullable(FrameHeight, hash, 53);
                    hash = EntityExtensions.HashNullable(FrameRate, hash, 53);
                    hash = EntityExtensions.HashNullable(FrameWidth, hash, 53);
                    hash = EntityExtensions.HashNullable(HorizontalAspectRatio, hash, 53);
                    hash = hash * 53 + StreamName.GetHashCode();
                    hash = EntityExtensions.HashNullable(StreamNumber, hash, 53);
                    hash = EntityExtensions.HashNullable(VerticalAspectRatio, hash, 53);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 53);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 53); ;
                    hash = hash * 53 + CreatedOn.GetHashCode();
                    hash = hash * 53 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
