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

        public override bool Equals(IImagePropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IImageProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            // TODO: Implement GetHashCode()
            throw new NotImplementedException();
        }
    }
}
