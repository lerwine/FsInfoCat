using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IMediaPropertiesRow" />
    /// <seealso cref="Local.Model.IMediaPropertiesRow" />
    public interface IUpstreamMediaPropertiesRow : IUpstreamPropertiesRow, M.IMediaPropertiesRow { }
}
