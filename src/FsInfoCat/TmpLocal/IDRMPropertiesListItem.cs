using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalDRMPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IDRMPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IDRMPropertiesListItem" />
    public interface ILocalDRMPropertiesListItem : ILocalDRMPropertiesRow, ILocalPropertiesListItem, M.IDRMPropertiesListItem { }
}
