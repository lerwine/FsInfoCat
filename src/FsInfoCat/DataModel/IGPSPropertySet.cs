using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file GPS property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="Local.ILocalGPSPropertySet" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet" />
    public interface IGPSPropertySet : IPropertySet, IGPSPropertiesRow, IEquatable<IGPSPropertySet> { }
}
