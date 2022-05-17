using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended image file property values.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IEquatable{ILocalImagePropertySet}" />
    /// <seealso cref="Upstream.IUpstreamImagePropertySet" />
    public interface ILocalImagePropertySet : ILocalImagePropertiesRow, ILocalPropertySet, IImagePropertySet, IEquatable<ILocalImagePropertySet> { }
}
