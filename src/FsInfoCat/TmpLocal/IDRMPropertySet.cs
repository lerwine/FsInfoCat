using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended DRM property values.
    /// </summary>
    /// <seealso cref="ILocalDRMPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IDRMPropertySet" />
    /// <seealso cref="IEquatable{ILocalDRMPropertySet}" />
    /// <seealso cref="Upstream.Model.IDRMPropertySet" />
    public interface ILocalDRMPropertySet : ILocalDRMPropertiesRow, ILocalPropertySet, M.IDRMPropertySet, IEquatable<ILocalDRMPropertySet> { }
}
