using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalDRMPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDRMPropertiesListItem" />
    public interface ILocalDRMPropertiesListItem : ILocalDRMPropertiesRow, ILocalPropertiesListItem, IDRMPropertiesListItem { }
}
