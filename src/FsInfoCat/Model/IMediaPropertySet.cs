using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of media files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="Local.Model.ILocalMediaPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamMediaPropertySet" />
    /// <seealso cref="IFile.MediaProperties" />
    /// <seealso cref="IDbContext.MediaPropertySets" />
    public interface IMediaPropertySet : IPropertySet, IMediaPropertiesRow, IEquatable<IMediaPropertySet> { }
}
