using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="M.IFileListItemWithBinaryProperties" />
    /// <seealso cref="IUpstreamFileRow" />
    /// <seealso cref="Local.Model.IFileListItemWithBinaryProperties" />
    public interface IUpstreamFileListItemWithBinaryProperties : IUpstreamDbFsItemListItem, M.IFileListItemWithBinaryProperties, IUpstreamFileRow { }
}
