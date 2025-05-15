using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file DRM property values.
    /// </summary>
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="IDbContext.DRMPropertySets"/>
    public interface IDRMPropertySet : IPropertySet, IDRMPropertiesRow, IEquatable<IDRMPropertySet> { }
}
