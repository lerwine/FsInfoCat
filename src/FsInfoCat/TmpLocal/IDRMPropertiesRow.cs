using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IDRMPropertiesRow" />
    /// <seealso cref="Upstream.Model.IDRMPropertiesRow" />
    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, M.IDRMPropertiesRow { }
}
