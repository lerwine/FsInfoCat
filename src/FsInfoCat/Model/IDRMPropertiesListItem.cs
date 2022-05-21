using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IEquatable{IDRMPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalDRMPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDRMPropertiesListItem" />
    /// <seealso cref="IDbContext.DRMPropertiesListing" />
    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMPropertiesRow, IEquatable<IDRMPropertiesListItem> { }
}
