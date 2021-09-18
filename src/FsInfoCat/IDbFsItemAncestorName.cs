namespace FsInfoCat
{
    public interface IDbFsItemAncestorName : IHasSimpleIdentifier
    {
        string Name { get; }

        string AncestorNames { get; }
    }
}

