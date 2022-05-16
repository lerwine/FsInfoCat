namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Local.ILocalMusicPropertiesRow" />
    public interface IUpstreamMusicPropertiesRow : IUpstreamPropertiesRow, IMusicPropertiesRow { }
}
