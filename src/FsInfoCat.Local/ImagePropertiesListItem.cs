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

        public bool Equals(ImagePropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = EntityExtensions.HashNullable(BitDepth, 47, 59);
                    hash = EntityExtensions.HashNullable(ColorSpace, hash, 59);
                    hash = EntityExtensions.HashNullable(CompressedBitsPerPixel, hash, 59);
                    hash = EntityExtensions.HashNullable(Compression, hash, 59);
                    hash = hash * 59 + CompressionText.GetHashCode();
                    hash = EntityExtensions.HashNullable(HorizontalResolution, hash, 59);
                    hash = EntityExtensions.HashNullable(HorizontalSize, hash, 59);
                    hash = hash * 59 + ImageID.GetHashCode();
                    hash = EntityExtensions.HashNullable(ResolutionUnit, hash, 59);
                    hash = EntityExtensions.HashNullable(VerticalResolution, hash, 59);
                    hash = EntityExtensions.HashNullable(VerticalSize, hash, 59);
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
