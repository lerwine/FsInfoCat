using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IDbContext.ImagePropertiesListing"/>
    public interface IImagePropertiesListItem : IPropertiesListItem, IImagePropertiesRow, IEquatable<IImagePropertiesListItem> { }
}
