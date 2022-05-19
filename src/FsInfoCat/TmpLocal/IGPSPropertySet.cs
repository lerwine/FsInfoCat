using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended GPS property values.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IGPSPropertySet" />
    /// <seealso cref="IEquatable{ILocalGPSPropertySet}" />
    /// <seealso cref="Upstream.Model.IGPSPropertySet" />
    public interface ILocalGPSPropertySet : ILocalGPSPropertiesRow, ILocalPropertySet, M.IGPSPropertySet, IEquatable<ILocalGPSPropertySet> { }
}
