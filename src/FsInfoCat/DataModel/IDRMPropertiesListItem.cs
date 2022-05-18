using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IEquatable{IDRMPropertiesListItem}" />
    /// <seealso cref="Local.ILocalDRMPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertiesListItem" />
    /// <seealso cref="IDbContext.DRMPropertiesListing" />
    public interface IDRMPropertiesListItem : IPropertiesListItem, IDRMPropertiesRow, IEquatable<IDRMPropertiesListItem> { }
}
