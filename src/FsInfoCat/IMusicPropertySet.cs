namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of music files.
    /// </summary>
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalMusicPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamMusicPropertySet"/>
    public interface IMusicPropertySet : IMusicProperties, IPropertySet
    {
    }
}
