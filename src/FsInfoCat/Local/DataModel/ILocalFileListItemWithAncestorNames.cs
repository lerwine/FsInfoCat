namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="ILocalFileRow" />
    public interface ILocalFileListItemWithAncestorNames : ILocalDbFsItemListItem, IFileListItemWithAncestorNames, ILocalFileRow { }
}