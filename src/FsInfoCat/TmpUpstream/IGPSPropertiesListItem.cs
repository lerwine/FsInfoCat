using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IGPSPropertiesListItem" />
    /// <seealso cref="Local.Model.IGPSPropertiesListItem" />
    public interface IUpstreamGPSPropertiesListItem : IUpstreamGPSPropertiesRow, IUpstreamPropertiesListItem, M.IGPSPropertiesListItem { }
}
