using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended image file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    public interface IUpstreamImagePropertySet : IUpstreamPropertySet, IImagePropertySet, IEquatable<IUpstreamImagePropertySet> { }
}
