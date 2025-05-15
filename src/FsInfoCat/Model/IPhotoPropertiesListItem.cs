using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IDbContext.PhotoPropertiesListing"/>
    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoPropertiesRow, IEquatable<IPhotoPropertiesListItem> { }
}
