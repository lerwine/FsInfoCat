using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IEquatable{ISummaryPropertiesListItem}" />
    /// <seealso cref="Local.ILocalSummaryPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertiesListItem" />
    /// <seealso cref="IDbContext.SummaryPropertiesListing" />
    [System.Obsolete("Use FsInfoCat.Model.ISummaryPropertiesListItem")]
    public interface ISummaryPropertiesListItem : IPropertiesListItem, ISummaryPropertiesRow, IEquatable<ISummaryPropertiesListItem> { }
}
