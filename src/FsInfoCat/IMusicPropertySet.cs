using System;

namespace FsInfoCat
{
    /// <summary>Interface for database objects that contain extended file property values of music files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMusicProperties" />
    public interface IMusicPropertySet : IPropertySet, IMusicProperties, IEquatable<IMusicPropertySet> { }
}
