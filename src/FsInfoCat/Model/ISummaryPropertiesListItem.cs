using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IDbContext.SummaryPropertiesListing"/>
    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryPropertiesRow, IEquatable<ISummaryPropertiesListItem> { }
}
