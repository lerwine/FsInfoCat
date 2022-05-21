using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamMusicPropertiesRow" />
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }
}
