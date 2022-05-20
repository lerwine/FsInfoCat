namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="ILocalDbFsItemListItem" />
    /// <seealso cref="ILocalFileRow" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.IUpstreamDbFsItemRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalDbFsItemRow")]
    public interface ILocalDbFsItemRow : ILocalDbEntity, IDbFsItemRow { }
}
