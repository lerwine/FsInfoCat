using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of music files.
    /// </summary>
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="IDbContext.MusicPropertySets"/>
    public interface IMusicPropertySet : IPropertySet, IMusicPropertiesRow, IEquatable<IMusicPropertySet> { }
}
