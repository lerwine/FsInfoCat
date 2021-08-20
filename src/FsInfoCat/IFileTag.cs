namespace FsInfoCat
{
    public interface IFileTag : IItemTag
    {
        new IFile Tagged { get; }
    }
}
