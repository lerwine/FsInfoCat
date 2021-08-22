namespace FsInfoCat.Local
{
    public interface ILocalItemTagRow : ILocalDbEntity, IItemTagRow { }

    public interface ILocalItemTagListItem : IItemTagListItem, ILocalItemTagRow { }

    public interface ILocalItemTag : ILocalItemTagRow, IItemTag
    {
        new ILocalDbEntity Tagged { get; }

        new ILocalTagDefinition Definition { get; }
    }
}
