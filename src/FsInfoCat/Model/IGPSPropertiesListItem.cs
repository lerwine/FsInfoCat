using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IDbContext.GPSPropertiesListing"/>
    public interface IGPSPropertiesListItem : IPropertiesListItem, IGPSPropertiesRow, IEquatable<IGPSPropertiesListItem> { }
}
