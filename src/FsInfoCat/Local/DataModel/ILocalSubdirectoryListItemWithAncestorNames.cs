namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    public interface ILocalSubdirectoryListItemWithAncestorNames : ISubdirectoryListItemWithAncestorNames, ILocalSubdirectoryRow { }
}
