using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IDRMPropertiesRow" />
    /// <seealso cref="Local.Model.IDRMPropertiesRow" />
    public interface IUpstreamDRMPropertiesRow : IUpstreamPropertiesRow, M.IDRMPropertiesRow { }
}
