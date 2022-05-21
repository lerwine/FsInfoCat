using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamDRMPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="Local.Model.IDRMPropertiesListItem" />
    public interface IUpstreamDRMPropertiesListItem : IUpstreamDRMPropertiesRow, IUpstreamPropertiesListItem, IDRMPropertiesListItem { }
}
