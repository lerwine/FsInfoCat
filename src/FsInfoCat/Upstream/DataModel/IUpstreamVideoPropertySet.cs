using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="IUpstreamVideoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamVideoPropertySet}" />
    /// <seealso cref="Local.ILocalVideoPropertySet" />
    public interface IUpstreamVideoPropertySet : IUpstreamVideoPropertiesRow, IUpstreamPropertySet, IVideoPropertySet, IEquatable<IUpstreamVideoPropertySet> { }
}
