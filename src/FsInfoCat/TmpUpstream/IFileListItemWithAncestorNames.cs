using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="M.IFileListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamFileRow" />
    /// <seealso cref="Local.Model.IFileListItemWithAncestorNames" />
    public interface IUpstreamFileListItemWithAncestorNames : IUpstreamDbFsItemListItem, M.IFileListItemWithAncestorNames, IUpstreamFileRow { }
}
