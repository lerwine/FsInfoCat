namespace FsInfoCat.Local
{
    public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag
    {
        new ILocalSubdirectory Tagged { get; }
    }
}
