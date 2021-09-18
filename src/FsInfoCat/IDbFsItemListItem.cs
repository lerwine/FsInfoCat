namespace FsInfoCat
{
    public interface IDbFsItemListItem : IDbFsItemRow
    {
        long AccessErrorCount { get; }

        long PersonalTagCount { get; }

        long SharedTagCount { get; }
    }
}

