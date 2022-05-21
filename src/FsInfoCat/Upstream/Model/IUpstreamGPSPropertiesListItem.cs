using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalGPSPropertiesListItem" />
    public interface IUpstreamGPSPropertiesListItem : IUpstreamGPSPropertiesRow, IUpstreamPropertiesListItem, IGPSPropertiesListItem { }
}
