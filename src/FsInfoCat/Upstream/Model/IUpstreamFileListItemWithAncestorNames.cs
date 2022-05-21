using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes the ancestor subdirectory names.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemListItem" />
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamFileRow" />
    /// <seealso cref="Local.Model.ILocalFileListItemWithAncestorNames" />
    public interface IUpstreamFileListItemWithAncestorNames : IUpstreamDbFsItemListItem, IFileListItemWithAncestorNames, IUpstreamFileRow { }
}
