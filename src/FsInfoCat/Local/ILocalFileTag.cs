namespace FsInfoCat.Local
{
    public interface ILocalFileTag : ILocalItemTag, IFileTag
    {
        new ILocalFile Tagged { get; }
    }
}
