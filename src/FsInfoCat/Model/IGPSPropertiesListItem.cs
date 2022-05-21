using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IEquatable{IGPSPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalGPSPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamGPSPropertiesListItem" />
    /// <seealso cref="IDbContext.GPSPropertiesListing" />
    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSPropertiesRow, IEquatable<IGPSPropertiesListItem> { }
}
