using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="Local.Model.IMusicPropertiesRow" />
    public interface IUpstreamMusicPropertiesRow : IUpstreamPropertiesRow, IMusicPropertiesRow { }
}
