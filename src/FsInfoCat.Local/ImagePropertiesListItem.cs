using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class ImagePropertiesListItem : ImagePropertiesRow, ILocalImagePropertiesListItem, IEquatable<ImagePropertiesListItem>
    {
        public const string VIEW_NAME = "vImagePropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<ImagePropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalImagePropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IImagePropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ImagePropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IImagePropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IImageProperties other)
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
                    hash = BitDepth.HasValue ? hash * 59 + (BitDepth ?? default).GetHashCode() : hash * 59;
                    hash = ColorSpace.HasValue ? hash * 59 + (ColorSpace ?? default).GetHashCode() : hash * 59;
                    hash = CompressedBitsPerPixel.HasValue ? hash * 59 + (CompressedBitsPerPixel ?? default).GetHashCode() : hash * 59;
                    hash = Compression.HasValue ? hash * 59 + (Compression ?? default).GetHashCode() : hash * 59;
                    hash = hash * 59 + CompressionText.GetHashCode();
                    hash = HorizontalResolution.HasValue ? hash * 59 + (HorizontalResolution ?? default).GetHashCode() : hash * 59;
                    hash = HorizontalSize.HasValue ? hash * 59 + (HorizontalSize ?? default).GetHashCode() : hash * 59;
                    hash = hash * 59 + ImageID.GetHashCode();
                    hash = ResolutionUnit.HasValue ? hash * 59 + (ResolutionUnit ?? default).GetHashCode() : hash * 59;
                    hash = VerticalResolution.HasValue ? hash * 59 + (VerticalResolution ?? default).GetHashCode() : hash * 59;
                    hash = VerticalSize.HasValue ? hash * 59 + (VerticalSize ?? default).GetHashCode() : hash * 59;
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
