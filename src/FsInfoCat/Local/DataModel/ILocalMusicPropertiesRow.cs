namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertiesRow" />
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }
}
