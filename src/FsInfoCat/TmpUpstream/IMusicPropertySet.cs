using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended music file property values.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.IMusicPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamMusicPropertySet}" />
    /// <seealso cref="Local.Model.IMusicPropertySet" />
    public interface IUpstreamMusicPropertySet : IUpstreamMusicPropertiesRow, IUpstreamPropertySet, M.IMusicPropertySet, IEquatable<IUpstreamMusicPropertySet> { }
}
