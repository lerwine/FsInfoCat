using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="M.ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.Model.ISubdirectoryListItemWithAncestorNames" />
    public interface IUpstreamSubdirectoryListItemWithAncestorNames : M.ISubdirectoryListItemWithAncestorNames, IUpstreamSubdirectoryRow { }
}
