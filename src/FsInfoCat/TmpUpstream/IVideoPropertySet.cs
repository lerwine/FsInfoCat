using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="IUpstreamVideoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.IVideoPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamVideoPropertySet}" />
    /// <seealso cref="Local.Model.IVideoPropertySet" />
    public interface IUpstreamVideoPropertySet : IUpstreamVideoPropertiesRow, IUpstreamPropertySet, M.IVideoPropertySet, IEquatable<IUpstreamVideoPropertySet> { }
}
