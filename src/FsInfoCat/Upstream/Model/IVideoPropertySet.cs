using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="IUpstreamVideoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamVideoPropertySet}" />
    /// <seealso cref="Local.Model.IVideoPropertySet" />
    public interface IUpstreamVideoPropertySet : IUpstreamVideoPropertiesRow, IUpstreamPropertySet, IVideoPropertySet, IEquatable<IUpstreamVideoPropertySet> { }
}
