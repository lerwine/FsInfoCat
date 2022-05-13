namespace FsInfoCat.Upstream
{
    public interface IUserProfileListItem : IUserProfileRow
    {
        long MemberOfCount { get; }

        long TaskCount { get; }
    }
}
