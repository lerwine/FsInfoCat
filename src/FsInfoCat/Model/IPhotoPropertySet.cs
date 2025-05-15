using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of photo files.
    /// </summary>
    /// <seealso cref="IPhotoPropertiesListItem" />
    /// <seealso cref="IDbContext.PhotoPropertySets"/>
    public interface IPhotoPropertySet : IPropertySet, IPhotoPropertiesRow, IEquatable<IPhotoPropertySet> { }
}
