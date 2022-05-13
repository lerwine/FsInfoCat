namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    public interface IUpstreamDocumentPropertiesRow : IUpstreamPropertiesRow, IDocumentPropertiesRow { }
}
