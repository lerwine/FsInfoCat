namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamFileRow" />
    /// <seealso cref="Local.ILocalFileListItemWithAncestorNames" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamFileListItemWithAncestorNames")]
    public interface IUpstreamFileListItemWithAncestorNames : IUpstreamDbFsItemListItem, IFileListItemWithAncestorNames, IUpstreamFileRow { }
}
