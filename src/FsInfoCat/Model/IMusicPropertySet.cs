using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of music files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="Local.Model.ILocalMusicPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamMusicPropertySet" />
    /// <seealso cref="IFile.MusicProperties" />
    /// <seealso cref="IDbContext.MusicPropertySets" />
    public interface IMusicPropertySet : IPropertySet, IMusicPropertiesRow, IEquatable<IMusicPropertySet> { }
}
