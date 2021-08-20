namespace FsInfoCat.Local
{
    public interface ILocalPersonalTag : ILocalItemTag, IPersonalTag
    {
        new ILocalPersonalTagDefinition Definition { get; }
    }
}
