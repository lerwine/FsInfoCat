using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    public interface IUpstreamSummaryPropertySet : IUpstreamPropertySet, ISummaryPropertySet, IEquatable<IUpstreamSummaryPropertySet> { }
}
