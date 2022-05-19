using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.ISummaryPropertySet" />
    /// <seealso cref="IEquatable{ILocalSummaryPropertySet}" />
    /// <seealso cref="Upstream.Model.ISummaryPropertySet" />
    public interface ILocalSummaryPropertySet : ILocalSummaryPropertiesRow, ILocalPropertySet, M.ISummaryPropertySet, IEquatable<ILocalSummaryPropertySet> { }
}
