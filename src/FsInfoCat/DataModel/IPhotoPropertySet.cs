using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of photo files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="Local.ILocalPhotoPropertySet" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertySet" />
    public interface IPhotoPropertySet : IPropertySet, IPhotoProperties, IEquatable<IPhotoPropertySet> { }
}
