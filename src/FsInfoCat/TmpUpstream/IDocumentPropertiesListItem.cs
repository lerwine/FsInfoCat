using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IDocumentPropertiesListItem" />
    /// <seealso cref="Local.Model.IDocumentPropertiesListItem" />
    public interface IUpstreamDocumentPropertiesListItem : IUpstreamDocumentPropertiesRow, IUpstreamPropertiesListItem, M.IDocumentPropertiesListItem { }
}
