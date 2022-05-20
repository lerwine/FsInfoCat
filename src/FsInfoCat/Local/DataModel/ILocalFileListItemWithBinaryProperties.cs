namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="Upstream.IUpstreamFileListItemWithBinaryProperties" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalFileListItemWithBinaryProperties")]
    public interface ILocalFileListItemWithBinaryProperties : ILocalDbFsItemListItem, IFileListItemWithBinaryProperties, ILocalFileRow { }
}
