using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="Local.Model.IMediaPropertiesRow" />
    public interface IUpstreamMediaPropertiesRow : IUpstreamPropertiesRow, IMediaPropertiesRow { }
}
