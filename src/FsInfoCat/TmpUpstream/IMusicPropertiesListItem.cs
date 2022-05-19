using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamMusicPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IMusicPropertiesListItem" />
    /// <seealso cref="Local.Model.IMusicPropertiesListItem" />
    public interface IUpstreamMusicPropertiesListItem : IUpstreamMusicPropertiesRow, IUpstreamPropertiesListItem, M.IMusicPropertiesListItem { }
}
