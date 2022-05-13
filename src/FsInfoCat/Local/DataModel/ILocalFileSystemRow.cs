namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IFileSystemRow" />
    public interface ILocalFileSystemRow : ILocalDbEntity, IFileSystemRow { }
}
