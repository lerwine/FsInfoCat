using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended image file property values.
    /// </summary>
    /// <seealso cref="IUpstreamImagePropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IEquatable{IUpstreamImagePropertySet}" />
    /// <seealso cref="Local.Model.ILocalImagePropertySet" />
    public interface IUpstreamImagePropertySet : IUpstreamImagePropertiesRow, IUpstreamPropertySet, IImagePropertySet, IEquatable<IUpstreamImagePropertySet> { }
}
