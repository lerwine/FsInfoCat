namespace FsInfoCat
{
    public interface ISubdirectoryTag : IItemTag
    {
        new ISubdirectory Tagged { get; }
    }
}
