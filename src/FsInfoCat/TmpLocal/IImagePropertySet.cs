using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended image file property values.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IImagePropertySet" />
    /// <seealso cref="IEquatable{ILocalImagePropertySet}" />
    /// <seealso cref="Upstream.Model.IImagePropertySet" />
    public interface ILocalImagePropertySet : ILocalImagePropertiesRow, ILocalPropertySet, M.IImagePropertySet, IEquatable<ILocalImagePropertySet> { }
}
