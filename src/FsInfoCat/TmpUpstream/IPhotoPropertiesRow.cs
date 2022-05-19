using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IPhotoPropertiesRow" />
    /// <seealso cref="Local.Model.IPhotoPropertiesRow" />
    public interface IUpstreamPhotoPropertiesRow : IUpstreamPropertiesRow, M.IPhotoPropertiesRow { }
}
