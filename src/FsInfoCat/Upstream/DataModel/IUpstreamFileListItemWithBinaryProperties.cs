namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="IUpstreamFileRow" />
    /// <seealso cref="Local.ILocalFileListItemWithBinaryProperties" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamFileListItemWithBinaryProperties")]
    public interface IUpstreamFileListItemWithBinaryProperties : IUpstreamDbFsItemListItem, IFileListItemWithBinaryProperties, IUpstreamFileRow { }
}
