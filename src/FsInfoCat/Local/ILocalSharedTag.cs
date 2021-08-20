namespace FsInfoCat.Local
{
    public interface ILocalSharedTag : ILocalItemTag, ISharedTag
    {
        new ILocalSharedTagDefinition Definition { get; }
    }
}
