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

        public override bool Equals(IPhotoProperties other)
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
                    int hash = 37;
                    hash = hash * 43 + CameraManufacturer.GetHashCode();
                    hash = hash * 43 + CameraModel.GetHashCode();
                    hash = DateTaken.HasValue ? hash * 43 + (DateTaken ?? default).GetHashCode() : hash * 43;
                    hash = (Event is null) ? hash * 43 : hash * 43 + (Event?.GetHashCode() ?? 0);
                    hash = hash * 43 + EXIFVersion.GetHashCode();
                    hash = Orientation.HasValue ? hash * 43 + (Orientation ?? default).GetHashCode() : hash * 43;
                    hash = hash * 43 + OrientationText.GetHashCode();
                    hash = (PeopleNames is null) ? hash * 43 : hash * 43 + (PeopleNames?.GetHashCode() ?? 0);
                    hash = UpstreamId.HasValue ? hash * 43 + (UpstreamId ?? default).GetHashCode() : hash * 43;
                    hash = LastSynchronizedOn.HasValue ? hash * 43 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 43;
                    hash = hash * 43 + CreatedOn.GetHashCode();
                    hash = hash * 43 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
