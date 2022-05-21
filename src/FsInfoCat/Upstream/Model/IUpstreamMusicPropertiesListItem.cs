using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalMusicPropertiesListItem" />
    public interface IUpstreamMusicPropertiesListItem : IUpstreamMusicPropertiesRow, IUpstreamPropertiesListItem, IMusicPropertiesListItem { }
}
