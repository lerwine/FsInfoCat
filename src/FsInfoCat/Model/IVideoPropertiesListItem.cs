using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IDbContext.VideoPropertiesListing"/>
    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoPropertiesRow, IEquatable<IVideoPropertiesListItem> { }
}
