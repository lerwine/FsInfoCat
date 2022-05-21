using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IEquatable{ISummaryPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalSummaryPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamSummaryPropertiesRow" />
    public interface ISummaryPropertiesRow : IPropertiesRow, ISummaryProperties, IEquatable<ISummaryPropertiesRow> { }
}
