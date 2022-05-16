using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="IUpstreamSummaryPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamSummaryPropertySet}" />
    /// <seealso cref="Local.ILocalSummaryPropertySet" />
    public interface IUpstreamSummaryPropertySet : IUpstreamSummaryPropertiesRow, IUpstreamPropertySet, ISummaryPropertySet, IEquatable<IUpstreamSummaryPropertySet> { }
}
