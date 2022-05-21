using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamGPSPropertySet}" />
    /// <seealso cref="Local.Model.ILocalGPSPropertySet" />
    public interface IUpstreamGPSPropertySet : IUpstreamGPSPropertiesRow, IUpstreamPropertySet, IGPSPropertySet, IEquatable<IUpstreamGPSPropertySet> { }
}
