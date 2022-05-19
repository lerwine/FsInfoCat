using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IEquatable{IPhotoPropertiesListItem}" />
    /// <seealso cref="Local.ILocalPhotoPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertiesListItem" />
    /// <seealso cref="IDbContext.PhotoPropertiesListing" />
    [System.Obsolete("Use FsInfoCat.Model.IPhotoPropertiesListItem")]
    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoPropertiesRow, IEquatable<IPhotoPropertiesListItem> { }
}
