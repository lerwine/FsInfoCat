using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of video files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="Local.ILocalVideoPropertySet" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertySet" />
    /// <seealso cref="IFile.VideoProperties" />
    /// <seealso cref="IDbContext.VideoPropertySets" />
    [System.Obsolete("Use FsInfoCat.Model.IVideoPropertySet")]
    public interface IVideoPropertySet : IPropertySet, IVideoPropertiesRow, IEquatable<IVideoPropertySet> { }
}
