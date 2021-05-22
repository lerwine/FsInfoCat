namespace FsInfoCat.Local
{
    public interface ILocalSymbolicName : ISymbolicName, ILocalDbEntity
    {
        new ILocalFileSystem FileSystem { get; set; }
    }
}
