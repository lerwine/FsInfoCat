namespace FsInfoCat
{
    public interface IPersonalTag : IItemTag
    {
        new IPersonalTagDefinition Definition { get; }
    }
}
