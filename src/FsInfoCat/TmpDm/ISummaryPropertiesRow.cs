using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IEquatable{ISummaryPropertiesRow}" />
    /// <seealso cref="Local.ILocalSummaryPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertiesRow" />
    public interface ISummaryPropertiesRow : IPropertiesRow, ISummaryProperties, IEquatable<ISummaryPropertiesRow> { }
}