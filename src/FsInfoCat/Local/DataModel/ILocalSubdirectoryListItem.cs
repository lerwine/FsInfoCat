namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    public interface ILocalSubdirectoryListItem : ISubdirectoryListItem, ILocalSubdirectoryRow { }
}
