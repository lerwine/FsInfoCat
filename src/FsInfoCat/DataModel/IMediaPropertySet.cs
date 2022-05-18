using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of media files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="Local.ILocalMediaPropertySet" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertySet" />
    /// <seealso cref="IFile.MediaProperties" />
    /// <seealso cref="IDbContext.MediaPropertySets" />
    public interface IMediaPropertySet : IPropertySet, IMediaPropertiesRow, IEquatable<IMediaPropertySet> { }
}
