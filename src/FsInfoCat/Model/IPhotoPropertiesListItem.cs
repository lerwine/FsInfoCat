using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IEquatable{IPhotoPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalPhotoPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamPhotoPropertiesListItem" />
    /// <seealso cref="IDbContext.PhotoPropertiesListing" />
    public interface IPhotoPropertiesListItem : IPropertiesListItem, IPhotoPropertiesRow, IEquatable<IPhotoPropertiesListItem> { }
}
