using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IDbContext.MediaPropertiesListing"/>
    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaPropertiesRow, IEquatable<IMediaPropertiesListItem> { }
}
