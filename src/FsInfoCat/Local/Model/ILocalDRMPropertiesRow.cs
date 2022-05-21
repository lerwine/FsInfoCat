using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamDRMPropertiesRow" />
    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, IDRMPropertiesRow { }
}
