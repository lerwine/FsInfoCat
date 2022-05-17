namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertiesRow" />
    public interface ILocalDocumentPropertiesRow : ILocalPropertiesRow, IDocumentPropertiesRow { }
}
