using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="IDbContext.RecordedTVPropertiesListing"/>
    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertiesListItem> { }
}
