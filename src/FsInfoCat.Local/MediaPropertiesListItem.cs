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
