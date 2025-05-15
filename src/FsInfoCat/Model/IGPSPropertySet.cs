using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file GPS property values.
    /// </summary>
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="IDbContext.GPSPropertySets"/>
    public interface IGPSPropertySet : IPropertySet, IGPSPropertiesRow, IEquatable<IGPSPropertySet> { }
}
