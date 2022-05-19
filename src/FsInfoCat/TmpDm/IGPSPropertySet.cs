using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file GPS property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="Local.ILocalGPSPropertySet" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet" />
    /// <seealso cref="IFile.GPSProperties" />
    /// <seealso cref="IDbContext.GPSPropertySets" />
    public interface IGPSPropertySet : IPropertySet, IGPSPropertiesRow, IEquatable<IGPSPropertySet> { }
}
