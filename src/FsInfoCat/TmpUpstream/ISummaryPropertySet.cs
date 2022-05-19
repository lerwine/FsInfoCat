using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="IUpstreamSummaryPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.ISummaryPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamSummaryPropertySet}" />
    /// <seealso cref="Local.Model.ISummaryPropertySet" />
    public interface IUpstreamSummaryPropertySet : IUpstreamSummaryPropertiesRow, IUpstreamPropertySet, M.ISummaryPropertySet, IEquatable<IUpstreamSummaryPropertySet> { }
}
