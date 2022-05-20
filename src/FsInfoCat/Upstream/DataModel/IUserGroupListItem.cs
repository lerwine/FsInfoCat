namespace FsInfoCat.Upstream
{
    // TODO: Document IUserGroupListItem interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUserGroupListItem")]
    public interface IUserGroupListItem : IUserGroupRow
    {
        long MemberCount { get; }

        long TaskCount { get; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
