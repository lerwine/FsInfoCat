namespace FsInfoCat
{
    public interface IItemTagListItem : IItemTagRow
    {
        string Name { get; }

        string Description { get; }
    }
}
