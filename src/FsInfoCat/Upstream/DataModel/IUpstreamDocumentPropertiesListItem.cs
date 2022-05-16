namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="Local.ILocalDocumentPropertiesListItem" />
    public interface IUpstreamDocumentPropertiesListItem : IUpstreamDocumentPropertiesRow, IUpstreamPropertiesListItem, IDocumentPropertiesListItem { }
}
