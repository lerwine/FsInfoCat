using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IEquatable{ILocalMusicPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertySet" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalMusicPropertySet")]
    public interface ILocalMusicPropertySet : ILocalMusicPropertiesRow, ILocalPropertySet, IMusicPropertySet, IEquatable<ILocalMusicPropertySet> { }
}
