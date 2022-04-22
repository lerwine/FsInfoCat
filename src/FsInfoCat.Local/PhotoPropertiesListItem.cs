using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class PhotoPropertiesListItem : PhotoPropertiesRow, ILocalPhotoPropertiesListItem, IEquatable<PhotoPropertiesListItem>
    {
        public const string VIEW_NAME = "vPhotoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PhotoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Event)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(PeopleNames)).HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalPhotoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPhotoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PhotoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPhotoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IPhotoPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IPhotoProperties other)
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
