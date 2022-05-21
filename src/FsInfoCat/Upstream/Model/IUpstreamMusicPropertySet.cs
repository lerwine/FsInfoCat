using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamMusicPropertySet}" />
    /// <seealso cref="Local.Model.ILocalMusicPropertySet" />
    public interface IUpstreamMusicPropertySet : IUpstreamMusicPropertiesRow, IUpstreamPropertySet, IMusicPropertySet, IEquatable<IUpstreamMusicPropertySet> { }
}
