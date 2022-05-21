using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IEquatable{ILocalGPSPropertySet}" />
    /// <seealso cref="Upstream.Model.IUpstreamGPSPropertySet" />
    public interface ILocalGPSPropertySet : ILocalGPSPropertiesRow, ILocalPropertySet, IGPSPropertySet, IEquatable<ILocalGPSPropertySet> { }
}
