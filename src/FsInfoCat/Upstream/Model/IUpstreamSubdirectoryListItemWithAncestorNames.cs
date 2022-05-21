using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.Model.ILocalSubdirectoryListItemWithAncestorNames" />
    public interface IUpstreamSubdirectoryListItemWithAncestorNames : ISubdirectoryListItemWithAncestorNames, IUpstreamSubdirectoryRow { }
}
