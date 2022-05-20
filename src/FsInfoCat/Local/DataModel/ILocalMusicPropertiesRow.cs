namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalMusicPropertiesRow")]
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }
}
