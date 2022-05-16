namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.ILocalSubdirectoryListItemWithAncestorNames" />
    public interface IUpstreamSubdirectoryListItemWithAncestorNames : ISubdirectoryListItemWithAncestorNames, IUpstreamSubdirectoryRow { }
}
