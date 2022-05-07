namespace FsInfoCat
{
    public interface ISharedTag : IItemTag
    {
        new ISharedTagDefinition Definition { get; }
    }
}
