using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file summary property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="Local.Model.ILocalSummaryPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamSummaryPropertySet" />
    /// <seealso cref="IFile.SummaryProperties" />
    /// <seealso cref="IDbContext.SummaryPropertySets" />
    public interface ISummaryPropertySet : IPropertySet, ISummaryPropertiesRow, IEquatable<ISummaryPropertySet> { }
}
