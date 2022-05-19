using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalMusicPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IMusicPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IMusicPropertiesListItem" />
    public interface ILocalMusicPropertiesListItem : ILocalMusicPropertiesRow, ILocalPropertiesListItem, M.IMusicPropertiesListItem { }
}
