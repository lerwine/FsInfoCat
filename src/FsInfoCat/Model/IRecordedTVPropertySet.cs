using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of recorded TV files.
    /// </summary>
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="IDbContext.RecordedTVPropertySets"/>
    public interface IRecordedTVPropertySet : IPropertySet, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertySet> { }
}
