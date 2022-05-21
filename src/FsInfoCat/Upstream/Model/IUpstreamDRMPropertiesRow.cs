using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalDRMPropertiesRow" />
    public interface IUpstreamDRMPropertiesRow : IUpstreamPropertiesRow, IDRMPropertiesRow { }
}
