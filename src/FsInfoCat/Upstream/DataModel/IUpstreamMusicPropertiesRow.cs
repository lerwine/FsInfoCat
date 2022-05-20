namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Local.ILocalMusicPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamMusicPropertiesRow")]
    public interface IUpstreamMusicPropertiesRow : IUpstreamPropertiesRow, IMusicPropertiesRow { }
}
