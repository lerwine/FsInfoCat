using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file summary property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="Local.ILocalSummaryPropertySet" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertySet" />
    public interface ISummaryPropertySet : IPropertySet, ISummaryPropertiesRow, IEquatable<ISummaryPropertySet> { }
}
