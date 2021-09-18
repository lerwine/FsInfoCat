namespace FsInfoCat
{
    public interface IItemTag : IItemTagRow
    {
        IDbEntity Tagged { get; }

        ITagDefinition Definition { get; }
    }
}
