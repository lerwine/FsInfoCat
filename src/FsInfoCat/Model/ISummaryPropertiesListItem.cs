using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IEquatable{ISummaryPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalSummaryPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamSummaryPropertiesListItem" />
    /// <seealso cref="IDbContext.SummaryPropertiesListing" />
    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryPropertiesRow, IEquatable<ISummaryPropertiesListItem> { }
}
