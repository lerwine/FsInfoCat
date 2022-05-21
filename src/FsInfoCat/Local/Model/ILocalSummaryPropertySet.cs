using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IEquatable{ILocalSummaryPropertySet}" />
    /// <seealso cref="Upstream.Model.IUpstreamSummaryPropertySet" />
    public interface ILocalSummaryPropertySet : ILocalSummaryPropertiesRow, ILocalPropertySet, ISummaryPropertySet, IEquatable<ILocalSummaryPropertySet> { }
}
