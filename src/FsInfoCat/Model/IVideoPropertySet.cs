using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of video files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="Local.Model.ILocalVideoPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamVideoPropertySet" />
    /// <seealso cref="IFile.VideoProperties" />
    /// <seealso cref="IDbContext.VideoPropertySets" />
    public interface IVideoPropertySet : IPropertySet, IVideoPropertiesRow, IEquatable<IVideoPropertySet> { }
}
