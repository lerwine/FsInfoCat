using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamPropertiesListItem" />
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, IPropertiesListItem { }
}
