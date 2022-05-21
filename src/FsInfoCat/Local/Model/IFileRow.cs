using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="Upstream.Model.IFileRow" />
    public interface ILocalFileRow : ILocalDbFsItemRow, IFileRow { }
}
