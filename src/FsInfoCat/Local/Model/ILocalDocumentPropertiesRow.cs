using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamDocumentPropertiesRow" />
    public interface ILocalDocumentPropertiesRow : ILocalPropertiesRow, IDocumentPropertiesRow { }
}
