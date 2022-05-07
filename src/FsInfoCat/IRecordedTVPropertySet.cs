using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IRecordedTVProperties" />
    public interface IRecordedTVPropertySet : IPropertySet, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertySet> { }
}
