using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamGPSPropertySet}" />
    /// <seealso cref="Local.ILocalGPSPropertySet" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamGPSPropertySet")]
    public interface IUpstreamGPSPropertySet : IUpstreamGPSPropertiesRow, IUpstreamPropertySet, IGPSPropertySet, IEquatable<IUpstreamGPSPropertySet> { }
}
