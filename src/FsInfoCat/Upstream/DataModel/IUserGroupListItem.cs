namespace FsInfoCat.Upstream
{
    public interface IUserGroupListItem : IUserGroupRow
    {
        long MemberCount { get; }

        long TaskCount { get; }
    }
}
