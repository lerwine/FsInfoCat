using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.IGPSPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamGPSPropertySet}" />
    /// <seealso cref="Local.Model.IGPSPropertySet" />
    public interface IUpstreamGPSPropertySet : IUpstreamGPSPropertiesRow, IUpstreamPropertySet, M.IGPSPropertySet, IEquatable<IUpstreamGPSPropertySet> { }
}
