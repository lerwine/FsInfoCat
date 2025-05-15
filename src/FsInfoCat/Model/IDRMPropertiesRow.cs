using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="IDRMPropertySet" />
    public interface IDRMPropertiesRow : IPropertiesRow, IDRMProperties, IEquatable<IDRMPropertiesRow> { }
}
