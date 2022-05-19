using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IEquatable{IMusicPropertiesRow}" />
    /// <seealso cref="Local.ILocalMusicPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IMusicPropertiesRow")]
    public interface IMusicPropertiesRow : IPropertiesRow, IMusicProperties, IEquatable<IMusicPropertiesRow> { }
}
