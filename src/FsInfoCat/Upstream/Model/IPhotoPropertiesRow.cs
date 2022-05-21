using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="Local.Model.IPhotoPropertiesRow" />
    public interface IUpstreamPhotoPropertiesRow : IUpstreamPropertiesRow, IPhotoPropertiesRow { }
}
