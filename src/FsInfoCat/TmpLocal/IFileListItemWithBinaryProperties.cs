using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="M.IFileListItemWithBinaryProperties" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="Upstream.Model.IFileListItemWithBinaryProperties" />
    public interface ILocalFileListItemWithBinaryProperties : ILocalDbFsItemListItem, M.IFileListItemWithBinaryProperties, ILocalFileRow { }
}
