using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Local.Model.IPropertiesListItem" />
    public interface IUpstreamPropertiesListItem : IUpstreamPropertiesRow, IPropertiesListItem { }
}
