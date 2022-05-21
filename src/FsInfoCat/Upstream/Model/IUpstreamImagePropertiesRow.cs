using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="Local.Model.ILocalImagePropertiesRow" />
    public interface IUpstreamImagePropertiesRow : IUpstreamPropertiesRow, IImagePropertiesRow { }
}
