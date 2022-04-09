using System;

namespace FsInfoCat
{
    /// <summary>Interface for database objects that contain extended file property values of media files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMediaProperties" />
    public interface IMediaPropertySet : IPropertySet, IMediaPropertiesRow, IEquatable<IMediaPropertySet> { }
}
