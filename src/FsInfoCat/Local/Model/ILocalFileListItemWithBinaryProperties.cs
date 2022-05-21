using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="Upstream.Model.IUpstreamFileListItemWithBinaryProperties" />
    public interface ILocalFileListItemWithBinaryProperties : ILocalDbFsItemListItem, IFileListItemWithBinaryProperties, ILocalFileRow { }
}
