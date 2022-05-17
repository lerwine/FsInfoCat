using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended DRM property values.
    /// </summary>
    /// <seealso cref="ILocalDRMPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IEquatable{ILocalDRMPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertySet" />
    public interface ILocalDRMPropertySet : ILocalDRMPropertiesRow, ILocalPropertySet, IDRMPropertySet, IEquatable<ILocalDRMPropertySet> { }
}
