using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file summary property values.
    /// </summary>
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="IDbContext.SummaryPropertySets"/>
    public interface ISummaryPropertySet : IPropertySet, ISummaryPropertiesRow, IEquatable<ISummaryPropertySet> { }
}
