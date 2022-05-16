using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended summary file property values.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IEquatable{ILocalSummaryPropertySet}" />
    public interface ILocalSummaryPropertySet : ILocalSummaryPropertiesRow, ILocalPropertySet, ISummaryPropertySet, IEquatable<ILocalSummaryPropertySet> { }
}
