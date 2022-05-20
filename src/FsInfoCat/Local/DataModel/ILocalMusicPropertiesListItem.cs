namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamMusicPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalMusicPropertiesListItem")]
    public interface ILocalMusicPropertiesListItem : ILocalMusicPropertiesRow, ILocalPropertiesListItem, IMusicPropertiesListItem { }
}
