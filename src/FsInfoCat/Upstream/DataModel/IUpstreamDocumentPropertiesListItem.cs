namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="Local.ILocalDocumentPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDocumentPropertiesListItem")]
    public interface IUpstreamDocumentPropertiesListItem : IUpstreamDocumentPropertiesRow, IUpstreamPropertiesListItem, IDocumentPropertiesListItem { }
}
