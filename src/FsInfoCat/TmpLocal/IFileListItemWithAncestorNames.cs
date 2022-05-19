using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="M.IFileListItemWithAncestorNames" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="Upstream.Model.IFileListItemWithAncestorNames" />
    public interface ILocalFileListItemWithAncestorNames : ILocalDbFsItemListItem, M.IFileListItemWithAncestorNames, ILocalFileRow { }
}
