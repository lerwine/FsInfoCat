using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IDbContext.DRMPropertiesListing"/>
    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMPropertiesRow, IEquatable<IDRMPropertiesListItem> { }
}
