using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended photo file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    public interface IUpstreamPhotoPropertySet : IUpstreamPropertySet, IPhotoPropertySet, IEquatable<IUpstreamPhotoPropertySet> { }
}
