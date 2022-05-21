using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="Upstream.Model.ISubdirectoryRow" />
    public interface ILocalSubdirectoryRow : ILocalDbFsItemRow, ISubdirectoryRow { }
}
