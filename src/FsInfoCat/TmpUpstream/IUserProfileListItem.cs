using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    // TODO: Document IUserProfileListItem interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IUserProfileListItem : IUserProfileRow
    {
        long MemberOfCount { get; }

        long TaskCount { get; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
