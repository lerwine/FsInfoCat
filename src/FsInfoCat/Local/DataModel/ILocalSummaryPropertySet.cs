using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    public interface ILocalSummaryPropertySet : ILocalSummaryPropertiesRow, ILocalPropertySet, ISummaryPropertySet, IEquatable<ILocalSummaryPropertySet> { }
}
