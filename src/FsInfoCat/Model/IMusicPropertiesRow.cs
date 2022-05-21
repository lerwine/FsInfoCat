using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IEquatable{IMusicPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalMusicPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamMusicPropertiesRow" />
    public interface IMusicPropertiesRow : IPropertiesRow, IMusicProperties, IEquatable<IMusicPropertiesRow> { }
}
