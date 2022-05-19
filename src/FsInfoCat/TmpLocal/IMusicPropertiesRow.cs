using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IMusicPropertiesRow" />
    /// <seealso cref="Upstream.Model.IMusicPropertiesRow" />
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, M.IMusicPropertiesRow { }
}
