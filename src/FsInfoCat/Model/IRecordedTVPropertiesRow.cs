using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertySet" />
    public interface IRecordedTVPropertiesRow : IPropertiesRow, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesRow> { }
}
