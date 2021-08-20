namespace FsInfoCat.Local
{
    public interface ILocalItemTag : ILocalDbEntity, IItemTag
    {
        new ILocalDbEntity Tagged { get; }

        new ILocalTagDefinition Definition { get; }
    }
}
