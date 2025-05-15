using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of media files.
    /// </summary>
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="IDbContext.MediaPropertySets"/>
    public interface IMediaPropertySet : IPropertySet, IMediaPropertiesRow, IEquatable<IMediaPropertySet> { }
}
