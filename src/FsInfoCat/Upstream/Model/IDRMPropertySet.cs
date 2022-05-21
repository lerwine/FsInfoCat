using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended DRM (Digital Rights Management) property values.
    /// </summary>
    /// <seealso cref="IUpstreamDRMPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamDRMPropertySet}" />
    /// <seealso cref="Local.Model.IDRMPropertySet" />
    public interface IUpstreamDRMPropertySet : IUpstreamDRMPropertiesRow, IUpstreamPropertySet, IDRMPropertySet, IEquatable<IUpstreamDRMPropertySet> { }
}
