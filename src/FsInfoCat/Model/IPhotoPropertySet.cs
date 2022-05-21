using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of photo files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="Local.Model.ILocalPhotoPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamPhotoPropertySet" />
    /// <seealso cref="IFile.PhotoProperties" />
    /// <seealso cref="IDbContext.PhotoPropertySets" />
    public interface IPhotoPropertySet : IPropertySet, IPhotoProperties, IEquatable<IPhotoPropertySet> { }
}
