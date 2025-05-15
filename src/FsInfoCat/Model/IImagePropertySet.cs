using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of image files.
    /// </summary>
    /// <seealso cref="IDbContext.ImagePropertySets"/>
    public interface IImagePropertySet : IPropertySet, IImagePropertiesRow, IEquatable<IImagePropertySet> { }
}
