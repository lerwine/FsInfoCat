using System;

namespace FsInfoCat
{
    /// <summary>Interface for database objects that contain extended file property values of image files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IImageProperties" />
    public interface IImagePropertySet : IPropertySet, IImageProperties, IEquatable<IImagePropertySet> { }
}
