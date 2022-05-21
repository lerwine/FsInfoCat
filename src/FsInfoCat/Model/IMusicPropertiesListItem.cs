using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IEquatable{IMusicPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalMusicPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamMusicPropertiesListItem" />
    /// <seealso cref="IDbContext.MusicPropertiesListing" />
    public interface IMusicPropertiesListItem : IPropertiesListItem, IMusicPropertiesRow, IEquatable<IMusicPropertiesListItem> { }
}
