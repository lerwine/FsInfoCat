namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="Local.ILocalMusicPropertiesListItem" />
    public interface IUpstreamMusicPropertiesListItem : IUpstreamMusicPropertiesRow, IUpstreamPropertiesListItem, IMusicPropertiesListItem { }
}
