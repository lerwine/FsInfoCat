using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IImagePropertiesRow" />
    /// <seealso cref="Local.Model.IImagePropertiesRow" />
    public interface IUpstreamImagePropertiesRow : IUpstreamPropertiesRow, M.IImagePropertiesRow { }
}
