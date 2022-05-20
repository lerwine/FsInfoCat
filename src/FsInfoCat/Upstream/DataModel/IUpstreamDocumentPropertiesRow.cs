namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="Local.ILocalDocumentPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDocumentPropertiesRow")]
    public interface IUpstreamDocumentPropertiesRow : IUpstreamPropertiesRow, IDocumentPropertiesRow { }
}
