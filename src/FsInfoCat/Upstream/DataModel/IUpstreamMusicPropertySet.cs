using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamMusicPropertySet}" />
    /// <seealso cref="Local.ILocalMusicPropertySet" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamMusicPropertySet")]
    public interface IUpstreamMusicPropertySet : IUpstreamMusicPropertiesRow, IUpstreamPropertySet, IMusicPropertySet, IEquatable<IUpstreamMusicPropertySet> { }
}
