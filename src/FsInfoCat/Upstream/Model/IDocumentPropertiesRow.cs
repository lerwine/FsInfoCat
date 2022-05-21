using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="Local.Model.IDocumentPropertiesRow" />
    public interface IUpstreamDocumentPropertiesRow : IUpstreamPropertiesRow, IDocumentPropertiesRow { }
}
