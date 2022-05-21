using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{IVideoPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalVideoPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamVideoPropertiesListItem" />
    /// <seealso cref="IDbContext.VideoPropertiesListing" />
    public interface IVideoPropertiesListItem : IPropertiesListItem, IVideoPropertiesRow, IEquatable<IVideoPropertiesListItem> { }
}
