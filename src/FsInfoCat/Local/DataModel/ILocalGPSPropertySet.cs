using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IEquatable{ILocalGPSPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet" />
    public interface ILocalGPSPropertySet : ILocalGPSPropertiesRow, ILocalPropertySet, IGPSPropertySet, IEquatable<ILocalGPSPropertySet> { }
}
