using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IDbContext.MusicPropertiesListing"/>
    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicPropertiesRow, IEquatable<IMusicPropertiesListItem> { }
}
