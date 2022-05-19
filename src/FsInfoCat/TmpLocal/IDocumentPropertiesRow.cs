using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IDocumentPropertiesRow" />
    /// <seealso cref="Upstream.Model.IDocumentPropertiesRow" />
    public interface ILocalDocumentPropertiesRow : ILocalPropertiesRow, M.IDocumentPropertiesRow { }
}
