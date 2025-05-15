using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of video files.
    /// </summary>
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="IDbContext.VideoPropertySets"/>
    public interface IVideoPropertySet : IPropertySet, IVideoPropertiesRow, IEquatable<IVideoPropertySet> { }
}
