namespace FsInfoCat.Upstream
{
    // TODO: Document IUserProfileListItem interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUserProfileListItem")]
    public interface IUserProfileListItem : IUserProfileRow
    {
        long MemberOfCount { get; }

        long TaskCount { get; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
