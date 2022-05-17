namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalDocumentPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertiesListItem" />
    public interface ILocalDocumentPropertiesListItem : ILocalDocumentPropertiesRow, ILocalPropertiesListItem, IDocumentPropertiesListItem { }
}
