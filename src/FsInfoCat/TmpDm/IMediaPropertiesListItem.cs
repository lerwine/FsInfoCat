using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IEquatable{IMediaPropertiesListItem}" />
    /// <seealso cref="Local.ILocalMediaPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertiesListItem" />
    /// <seealso cref="IDbContext.MediaPropertiesListing" />
    public interface IMediaPropertiesListItem : IPropertiesListItem, IMediaPropertiesRow, IEquatable<IMediaPropertiesListItem> { }
}
