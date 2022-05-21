using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="Upstream.Model.IUpstreamFileListItemWithAncestorNames" />
    public interface ILocalFileListItemWithAncestorNames : ILocalDbFsItemListItem, IFileListItemWithAncestorNames, ILocalFileRow { }
}
