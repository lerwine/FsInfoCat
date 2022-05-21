using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended image file property values.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IEquatable{ILocalImagePropertySet}" />
    /// <seealso cref="Upstream.Model.IUpstreamImagePropertySet" />
    public interface ILocalImagePropertySet : ILocalImagePropertiesRow, ILocalPropertySet, IImagePropertySet, IEquatable<ILocalImagePropertySet> { }
}
