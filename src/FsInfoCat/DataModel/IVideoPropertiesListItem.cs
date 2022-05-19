using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{IVideoPropertiesListItem}" />
    /// <seealso cref="Local.ILocalVideoPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertiesListItem" />
    /// <seealso cref="IDbContext.VideoPropertiesListing" />
    [System.Obsolete("Use FsInfoCat.Model.IVideoPropertiesListItem")]
    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoPropertiesRow, IEquatable<IVideoPropertiesListItem> { }
}
